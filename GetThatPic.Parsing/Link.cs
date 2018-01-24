// <copyright file="Link.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

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
                    Images = new List<IImageDownloadInstruction>()
                    {
                        new ImageDownloadFromMarkup()
                        {
                            Type = DomElementAccessor.TargetType.Attribute,
                            AttributeName = "src",
                            Selector = ".img-comic"
                        }
                    }
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
            Domain domain = IdentifyDomain(url);

            HtmlDocument doc = await GetDocument(url);

            IList<string> imagePaths;
            foreach (IImageDownloadInstruction downloadInstruction in domain.Images)
            {
                // TODO: improve typing.
                imagePaths = GetContent(doc, (DomElementAccessor)downloadInstruction);
                if (null != imagePaths)
                {
                    return imagePaths;
                }
            }

            return null;
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

            Stream stream = await requester.GetStream(url);
            var doc = new HtmlDocument();
            doc.Load(stream);

            return doc;
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

        /// <summary>
        /// Gets the content specified by a DomElementAccessor from a given HtmlDocument.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="accessor">The accessor.</param>
        /// <returns>The desired Content.</returns>
        public IList<string> GetContent(HtmlDocument doc, DomElementAccessor accessor)
        {
            if (null == doc || null == accessor || !accessor.IsValid)
            {
                return null;
            }

            IList<HtmlNode> nodes = doc.QuerySelectorAll(accessor.Selector);
            IList<string> output = null;

            switch (accessor.Type)
            {
                case DomElementAccessor.TargetType.Html:
                    output = nodes.Select(node => node.InnerHtml).ToList();
                    break;

                case DomElementAccessor.TargetType.Text:
                    output = nodes.Select(node => node.InnerText).ToList();
                    break;

                case DomElementAccessor.TargetType.Attribute:
                    output = nodes.Select(
                        node => node.Attributes.First(
                            attribute => accessor.AttributeName == attribute.Name).Value).ToList();
                    break;
            }

            output = output?.Select(item => accessor.Pattern.Replace(item, accessor.Replace)).ToList();
            return output;
        }
    }
}
