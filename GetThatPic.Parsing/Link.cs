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
using GetThatPic.Parsing.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetThatPic.Parsing
{
    /// <summary>
    /// Parses an url for the correct domain configuration node.
    /// From Domain and url other data can be retreived.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// The protocol finding pattern.
        /// </summary>
        private static readonly Regex Protocol = new Regex("^(https?:).*?$", RegexOptions.IgnoreCase);

        /// <summary>
        /// The HTML comment pattern to drop them.
        /// </summary>
        private static readonly Regex HtmlCommentPattern = new Regex("<!--(.|\n)*?--!?>", RegexOptions.Multiline);

        /// <summary>
        /// Pattern to drop a leading period.
        /// </summary>
        private static readonly Regex DropLeadingPeriod = new Regex(@"^\.(.*)");

        /// <summary>
        /// Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="autoInitialize">if set to <c>true</c> the config gets automatically initialized.</param>
        /// <param name="loadCustomConfigs">if set to <c>true</c> [load custom configs].</param>
        public Link(bool autoInitialize = true, bool loadCustomConfigs = true)
        {
            if (autoInitialize)
            {
                InitializeConfig(loadCustomConfigs: loadCustomConfigs);
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
            
            string markup = await HttpRequester.GetString(url);
            markup = HtmlCommentPattern.Replace(markup, string.Empty);
            return GetDocumentFromMarkup(markup);
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
            if (doc.ParseErrors.Any(error => HtmlParseErrorCode.TagNotOpened != error.Code && HtmlParseErrorCode.TagNotClosed != error.Code && HtmlParseErrorCode.EndTagNotRequired != error.Code))
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
        /// <param name="includePeriod">if set to <c>true</c> the period should be included.</param>
        /// <returns>
        /// The file ending.
        /// </returns>
        public static string FileEndingFromUrl(string url, bool includePeriod = true)
        {
            string ending = Path.GetExtension(url) ?? string.Empty;
            if (!includePeriod)
            {
                return DropLeadingPeriod.Replace(ending, "$1");
            }

            return ending;
            /*
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            
            Regex fileEnding = new Regex(@"^.*?(\.[a-zA-Z0-9]+)$");
            string output = fileEnding.Replace(url, "$1");
            return url != output ? output : string.Empty;
            */
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="clearFirst">if set to <c>true</c> clear the config before initializing.</param>
        /// <param name="loadCustomConfigs">if set to <c>true</c> [load custom configs].</param>
        public void InitializeConfig(bool clearFirst = true, bool loadCustomConfigs = true)
        {
            if (clearFirst)
            {
                Domains.Clear();
            }

            IList<Domain> domains = LoadDomainsFromJsonFile("Domains", loadCustomConfigs);
            IList<DownloadDirectory> downloadDirectories = LoadDomainDirectoriesFromJsonFile("DownloadDirectories", loadCustomConfigs);
            
            foreach (Domain domain in domains)
            {
                DownloadDirectory directoryConfig = downloadDirectories
                    .FirstOrDefault(downloadDirectory => downloadDirectory.Name == domain.Name);
                string directory = directoryConfig?.Directory;
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    domain.DownloadDirectory = directory;
                }

                if (null != directoryConfig?.IsPathRelative)
                {
                    domain.IsPathRelative = directoryConfig.IsPathRelative;
                }

                Domains.Add(domain);
            }
        }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A Listof corresponding ImageEntries.</returns>
        public async Task<IList<ImageMetaData>> GetImages(string url)
        {
            Domain domain = IdentifyDomain(url);
            HtmlDocument doc = await GetDocument(url);

            IEnumerable<string> imageUrls = (await GetImageUrls(url, doc, domain)).ToList();
            IEnumerable<string> fileEndings = imageUrls.Select(url1 => FileEndingFromUrl(url1))
                .Select(ending => !string.IsNullOrWhiteSpace(ending) ? ending : domain.DefaultFileEnding).ToList();

            string imageBaseFileName = await GetImageFileName(url, doc, domain);

            IList<string> imageFileNames = new List<string>();
            int numImages = imageUrls.Count();
            if (numImages > 1)
            {
                int numDigits = (numImages + 1).ToString().Length;
                for (int i = 1; i <= numImages; i++)
                {
                    string fileName = imageBaseFileName + i.ToString().PadLeft(numDigits, '0') +
                                      fileEndings.ElementAt(i - 1);
                    imageFileNames.Add(fileName);
                }
            }
            else if (numImages == 1)
            {
                imageFileNames.Add(imageBaseFileName + fileEndings.ElementAt(0));
            }

            IList<ImageMetaData> entries = imageUrls.Zip(
                imageFileNames,
                (imageUrl, name) => new ImageMetaData()
                {
                    ImageUrl = imageUrl,
                    Name = name,
                    TargetFileSystemLocation = domain.DownloadDirectory + name
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
        public async Task<IEnumerable<string>> GetImageUrls(string url, HtmlDocument doc = null, Domain domain = null)
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
                IEnumerable<string> imagePaths = downloadInstruction.GetContent(doc, url);
                if (imagePaths.Any())
                {
                    string protocol = Protocol.Replace(url, "$1");
                    imagePaths = imagePaths.Select(imageUrl => imageUrl.StartsWith("//") ? protocol + imageUrl : imageUrl);
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
                imageNameFragments.Add(downloadInstruction.GetContent(doc, url));
            }

            IEnumerable<string> nonEmptyNameParts = imageNameFragments.SelectMany(fragments => fragments)
                .Where(fragment => !string.IsNullOrWhiteSpace(fragment));
            string joinedFragments = string.Join(domain.FileNameFragmentDelimiter, nonEmptyNameParts);
            return Sanitizing.SanititzeFileName(joinedFragments);
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

            IList<Domain> matchingDomains = Domains.Where(
                d => d.Url.IsMatch(domain) 
                     && d.Path.IsMatch(path)).ToList();

            return matchingDomains.FirstOrDefault();
        }

        /// <summary>
        /// Parses the configuration json.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="loadCustomConfigs">if set to <c>true</c> [load custom configs].</param>
        /// <returns>The JSON-string for the config.</returns>
        private static string ParseConfigJson(string fileName, bool loadCustomConfigs)
        {
            string inputFile = FindJsonFile(fileName);
            string json;
            using (StreamReader r = new StreamReader(inputFile))
            {
                json = r.ReadToEnd();
            }

            var jsonData = JArray.Parse(json);
            
            if (loadCustomConfigs)
            {
                string customInputFile = FindJsonFile(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) +
                                                      @"\GetThatPic\Config\" + fileName);
                if (!string.IsNullOrWhiteSpace(customInputFile))
                {
                    string customJson;
                    using (StreamReader r = new StreamReader(customInputFile))
                    {
                        customJson = r.ReadToEnd();
                    }

                    var customJsonData = JArray.Parse(customJson);
                    var mergeSettings = new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    };

                    customJsonData.Merge(jsonData, mergeSettings);
                    jsonData = customJsonData;
                }
            }

            return JsonConvert.SerializeObject(jsonData);
        }

        /// <summary>
        /// Finds the json file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The identified file name.</returns>
        private static string FindJsonFile(string fileName)
        {
            if (File.Exists(@"..\..\..\..\" + fileName + ".json"))
            {
                return @"..\..\..\..\" + fileName + ".json";
            }

            if (File.Exists(@"..\..\..\" + fileName + ".json"))
            {
                return @"..\..\..\" + fileName + ".json";
            }

            if (File.Exists(fileName + ".json"))
            {
                return fileName + ".json";
            }

            if (File.Exists(fileName))
            {
                return fileName;
            }

            return null;
        }

        /// <summary>
        /// Loads the domains from json file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="loadCustomConfigs">if set to <c>true</c> load custom configs.</param>
        /// <returns>
        /// The loaded domains.
        /// </returns>
        private IList<Domain> LoadDomainsFromJsonFile(string fileName = null, bool loadCustomConfigs = true)
        {
            string json = ParseConfigJson(fileName, loadCustomConfigs);
            var domains = JsonConvert.DeserializeObject<List<Domain>>(json);
            return domains;
        }

        /// <summary>
        /// Loads the domains from json file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="loadCustomConfigs">if set to <c>true</c> [load custom configs].</param>
        /// <returns>
        /// The loaded domains.
        /// </returns>
        private IList<DownloadDirectory> LoadDomainDirectoriesFromJsonFile(string fileName = null, bool loadCustomConfigs = true)
        {
            string json = ParseConfigJson(fileName, loadCustomConfigs);
            var downloadDirectories = JsonConvert.DeserializeObject<List<DownloadDirectory>>(json);
            return downloadDirectories;
        }
    }
}