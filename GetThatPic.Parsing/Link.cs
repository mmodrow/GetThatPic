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
using GetThatPic.Parsing.Models;
using HtmlAgilityPack;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Parses an url for the correct domain configuration node.
    /// From Domain and url other data can be retreived.
    /// </summary>
    public class Link
    {
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
        }

        /// <summary>
        /// Gets the domain configruation nodes.
        /// </summary>
        /// <value>
        /// The domains.
        /// </value>
        public IList<Domain> Domains { get; } = new List<Domain>();

        /// <summary>
        /// Gets the http document from an URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The parsed HtmlDocument.</returns>
        public static async Task<HtmlDocument> GetDocumentFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                Stream stream = await HttpRequester.GetStream(url);
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
        public static HtmlDocument GetDocumentFromMarkup(string markup)
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
        public static async Task<HtmlDocument> GetDocument(string input)
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
        /// Get the file ending from an URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The file ending.</returns>
        public static string FileEndingFromUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            Regex fileEnding = new Regex(@"^.*?(\.[a-zA-Z0-9]+)$");
            string output = fileEnding.Replace(url, "$1");
            return url != output ? output : string.Empty;
        }

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
                domains = DefaultConfig.Domains;
            }

            foreach (Domain domain in domains)
            {
                Domains.Add(domain);
            }
        }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A Listof corresponding ImageEntries.</returns>
        public async Task<IList<ImageEntry>> GetImages(string url)
        {
            Domain domain = IdentifyDomain(url);
            HtmlDocument doc = await GetDocument(url);

            IList<string> imageUrls = await GetImageUrls(url, doc, domain);
            IList<string> fileEndings = imageUrls.Select(FileEndingFromUrl).ToList();

            string imageBaseFileName = await GetImageFileName(url, doc, domain);

            IList<string> imageFileNames = new List<string>();
            if (imageUrls.Count > 1)
            {
                int numImages = imageUrls.Count;
                int numDigits = (numImages + 1).ToString().Length;
                for (int i = 1; i <= numImages; i++)
                {
                    string fileName = imageBaseFileName + i.ToString().PadLeft(numDigits, '0') +
                                      fileEndings.ElementAt(i - 1);
                    imageFileNames.Add(fileName);
                }
            }
            else
            {
                imageFileNames.Add(imageBaseFileName);
            }

            IList<ImageEntry> entries = imageUrls.Zip(
                imageFileNames,
                (imageUrl, name) => new ImageEntry()
                {
                    Content = HttpRequester.GetImage(imageUrl),
                    Name = name,
                    FileSystemLocation = imageUrl
                }).ToList();

            return entries;
        }

        /// <summary>
        /// Gets the image urls for a given input url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="doc">The document to parse.</param>
        /// <param name="domain">The domain to parse against.</param>
        /// <returns>List of Image Urls</returns>
        public async Task<IList<string>> GetImageUrls(string url, HtmlDocument doc = null, Domain domain = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new List<string>();
            }

            if (null == domain)
            {
                domain = IdentifyDomain(url);
            }

            if (null == domain || !domain.Images.Any())
            {
                return new List<string>();
            }

            if (null == doc)
            {
                doc = await GetDocument(url);
            }

            if (null == doc)
            {
                return new List<string>();
            }

            foreach (IContentAccessor downloadInstruction in domain.Images)
            {
                IList<string> imagePaths = downloadInstruction.GetContent(doc);
                if (imagePaths.Any())
                {
                    return imagePaths;
                }
            }

            return new List<string>();
        }

        /// <summary>
        /// Gets the image file name for a given input url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="doc">The document to parse.</param>
        /// <param name="domain">The domain to parse against.</param>
        /// <returns>List of Image Urls</returns>
        public async Task<string> GetImageFileName(string url, HtmlDocument doc = null, Domain domain = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            if (null == domain)
            {
                domain = IdentifyDomain(url);
            }

            if (null == domain || !domain.FileNameFragments.Any())
            {
                return string.Empty;
            }

            if (null == doc)
            {
                doc = await GetDocument(url);
            }

            if (null == doc)
            {
                return string.Empty;
            }

            IList<IList<string>> imageNameFragments = new List<IList<string>>();

            foreach (IContentAccessor downloadInstruction in domain.FileNameFragments)
            {
                imageNameFragments.Add(downloadInstruction.GetContent(doc));
            }

            return FileNameSanitizing.Sanititze(
                string.Join(
                    domain.FileNameFragmentDelimiter,
                    imageNameFragments.SelectMany(fragments => fragments)));
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
    }
}