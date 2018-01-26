// <copyright file="Link.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetThatPic.Data.Configuration;
using GetThatPic.Data.IO;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Parses an url for the correct domain configuration node.
    /// From Domain and url other data can be retreived.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// The http requester.
        /// </summary>
        private readonly HttpRequester requester;

        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="autoInitialize">if set to <c>true</c> the config gets automatically initialized.</param>
        public Link(bool autoInitialize = true)
        {
            if (autoInitialize)
            {
                InitializeConfig();
            }

            requester = new HttpRequester();
        }

        /// <summary>
        /// Gets the domain configruation nodes.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        public IList<Domain> Domains { get; } = new List<Domain>();

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="clearFirst">if set to <c>true</c> clear the config before initializing.</param>
        /// <param name="domains">The domains.</param>
        public void InitializeConfig(bool clearFirst = true, IList<Domain> domains = null)
        {
            if (clearFirst)
            {
                Domains.Clear();
            }

            if (null == domains)
            {
                Domains.Add(new Domain
                {
                    Name = "dilbert.com",
                    Url = "http://dilbert.com",
                    Path = new Regex("^/strip/((?:[0-9]+-?)+)$"),
                    Images = new List<IContentAccessor>()
                    {
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Attribute,
                            AttributeName = "src",
                            Selector = ".img-comic"
                        }
                    },
                    FileNameFragments = new List<IContentAccessor>()
                    {
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Text,
                            Selector = "title",
                            Pattern = new Regex(@"^.*?-  Dilbert Comic Strip on (\d{4}-\d{2}-\d{2}).*$")
                        },
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Text,
                            Selector = ".comic-title-name"
                        }
                    },
                    FileNameFragmentDelimiter = "_-_"
                });

                Domains.Add(new Domain
                {
                    Name = "www.schisslaweng.net",
                    Url = "https://www.schisslaweng.net",
                    Path = new Regex("^/(.*?)/.*$"),
                    Images = new List<IContentAccessor>()
                    {
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Attribute,
                            AttributeName = "src",
                            Selector = ".gallery-item img"
                        }
                    },
                    FileNameFragments = new List<IContentAccessor>()
                    {
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Attribute,
                            AttributeName = "content",
                            Selector = @"meta[name=""shareaholic:article_published_time""]",
                            Pattern = new Regex(@"^(\d{4}-\d{2}-\d{2}).*$")
                        },
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Text,
                            Selector = "h1"
                        }
                    },
                    FileNameFragmentDelimiter = "_-_"
                });
            }
            else
            {
                foreach (Domain domain in domains)
                {
                    Domains.Add(domain);
                }
            }
        }

        /// <summary>
        /// Gets the image urls for a given input url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>List of Image Urls</returns>
        public async Task<IList<string>> GetImageUrls(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            Domain domain = IdentifyDomain(url);

            if (null == domain || !domain.Images.Any())
            {
                return null;
            }

            HtmlDocument doc = await GetDocument(url);

            if (null == doc)
            {
                return null;
            }

            foreach (IContentAccessor downloadInstruction in domain.Images)
            {
                IList<string> imagePaths = downloadInstruction.GetContent(doc);
                if (imagePaths.Any())
                {
                    return imagePaths;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the image file name for a given input url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>List of Image Urls</returns>
        public async Task<string> GetImageFileName(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            Domain domain = IdentifyDomain(url);

            if (null == domain || !domain.FileNameFragments.Any())
            {
                return null;
            }

            HtmlDocument doc = await GetDocument(url);

            if (null == doc)
            {
                return null;
            }

            IList<IList<string>> imageNameFragments = new List<IList<string>>();

            foreach (IContentAccessor downloadInstruction in domain.FileNameFragments)
            {
                imageNameFragments.Add(downloadInstruction.GetContent(doc));
            }

            return string.Join(domain.FileNameFragmentDelimiter, imageNameFragments.SelectMany(fragments => fragments));
        }

        /// <summary>
        /// Identifies the domain fro a given uri.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The identified Domain configuration node or null if none applies.</returns>
        public Domain IdentifyDomain(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Domains.Any())
            {
                return null;
            }

            string domain = HttpRequester.ProtocolAndDomain.Replace(url, "$1");
            string path = HttpRequester.PathAfterdomain.Replace(url, "$1");
            IList<Domain> matchingDomains = Domains.Where(d => d.Url == domain && d.Path.IsMatch(path)).ToList();

            return matchingDomains.FirstOrDefault();
        }

        /// <summary>
        /// Gets the http document from an URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The parsed HtmlDocument.</returns>
        public async Task<HtmlDocument> GetDocumentFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                Stream stream = await requester.GetStream(url);
                var doc = new HtmlDocument();
                doc.Load(stream);

                return doc;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the http document from given markup.
        /// </summary>
        /// <param name="markup">The markup.</param>
        /// <returns>The parsed HtmlDocument.</returns>
        public HtmlDocument GetDocumentFromMarkup(string markup)
        {
            if (string.IsNullOrEmpty(markup))
            {
                return null;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(markup);
            if (doc.ParseErrors.Any())
            {
                return null;
            }

            return doc;
        }

        /// <summary>
        /// Gets a document from either an url or markup.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The parsed HtmlDocument.</returns>
        public async Task<HtmlDocument> GetDocument(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (HttpRequester.ProtocolAndDomain.IsMatch(input))
            {
                return await GetDocumentFromUrl(input);
            }

            return GetDocumentFromMarkup(input);
        }
    }
}
