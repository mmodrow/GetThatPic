// <copyright file="LinkTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GetThatPic.Data.Configuration;
using GetThatPic.Parsing;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Xunit;

namespace GetThatPic.Test.Parsing
{
    /// <summary>
    /// Tests functionality of the Link class.
    /// </summary>
    public class LinkTests
    {
        /// <summary>
        /// The valid markup example.
        /// </summary>
        private const string ValidMarkup            = @"<html>
    <head>
        <title>groﬂartig</title>
    </head>
    <body>
        <h1>toll!</h1>
        <nav class=""MainNav""><span id=""MainNavChild"" data-stuff=""Dreams are made of"">fun</span></nav>
        <h2>first second headline</h2>
        <h2>second second headline</h2>
    </body>
</html>";

        /// <summary>
        /// The broken markup example.
        /// </summary>
        private const string BrokenMarkup            = "<html<head><title>groﬂartig</title>";

        /// <summary>
        /// Tests the constructor behaviour when passing true.
        /// </summary>
        [Fact]
        public void Constructor_True()
        {
            Link link = new Link(true);

            Assert.Contains(link.Domains, domain => "http://dilbert.com" == domain.Url);
        }

        /// <summary>
        /// Tests the constructor behaviour when passing false.
        /// </summary>
        [Fact]
        public void Constructor_False()
        {
            Link link = new Link(false);

            Assert.DoesNotContain(link.Domains, domain => "http://dilbert.com" == domain.Url);
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: empty configuration, non empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_EmptyConfig_NonEmptyUrl()
        {
            Link link = new Link(false);

            Assert.Null(link.IdentifyDomain("http://dilbert.com"));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: empty configuration, empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_EmptyConfig_EmptyUrl()
        {
            Link link = new Link(false);

            Assert.Null(link.IdentifyDomain(null));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: non empty configuration, empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_NonEmptyConfig_EmptyUrl()
        {
            Link link = new Link(false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });

            Assert.Null(link.IdentifyDomain(null));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: non empty configuration, non empty URL, Url contained.
        /// </summary>
        [Fact]
        public void IdentifyDomain_NonEmptyConfig_NonEmptyUrl_Contained()
        {
            Link link = new Link(false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });

            Assert.Equal("http://dilbert.com", link.IdentifyDomain("http://dilbert.com/strip/2018-01-22")?.Url);
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: non empty configuration, non empty URL, Url not contained.
        /// </summary>
        [Fact]
        public void IdentifyDomain_NonEmptyConfig_NonEmptyUrl_NotContained()
        {
            Link link = new Link(false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });

            Assert.Null(link.IdentifyDomain("https://google.com")?.Url);
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with an element given.
        /// </summary>
        [Fact]
        public void InitializeConfig_ElementAdded()
        {
            Link link = new Link(false);
            link.InitializeConfig(
                true,
                new List<Domain>
                {
                    new Domain
                    {
                        Name = "dilbert.com",
                        Url = "http://dilbert.com",
                        Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
                    }
                });

            Assert.Contains(link.Domains, domain => "http://dilbert.com" == domain.Url);
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with no element given.
        /// </summary>
        [Fact]
        public void InitializeConfig_NoElementAdded()
        {
            Link link = new Link(false);
            link.InitializeConfig();

            Assert.Contains(link.Domains, domain => "http://dilbert.com" == domain.Url);
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with an empty list given.
        /// </summary>
        [Fact]
        public void InitializeConfig_EmptyListAdded()
        {
            Link link = new Link(false);
            link.InitializeConfig(true, new List<Domain>());

            Assert.DoesNotContain(link.Domains, domain => "http://dilbert.com" == domain.Url);
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_Title()
        {
            HtmlDocument doc = await Link.GetDocumentFromUrl("https://google.com/");
            IList<HtmlNode> title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "Google" == item.InnerHtml);
        }

        /// <summary>
        /// Gets null.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_null()
        {
            HtmlDocument doc = await Link.GetDocumentFromUrl(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_empty()
        {
            HtmlDocument doc = await Link.GetDocumentFromUrl(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Title()
        {
            HtmlDocument doc = Link.GetDocumentFromMarkup(ValidMarkup);
            IList<HtmlNode> title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "groﬂartig" == item.InnerHtml);
        }

        /// <summary>
        /// Gets null.
        /// </summary>
        [Fact]
        public  void GetDocumentFromMarkup_null()
        {
            HtmlDocument doc = Link.GetDocumentFromMarkup(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Empty()
        {
            HtmlDocument doc = Link.GetDocumentFromMarkup(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an empty string.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Invalid()
        {
            HtmlDocument doc = Link.GetDocumentFromMarkup(BrokenMarkup);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the document URL.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetDocument_Url()
        {
            HtmlDocument doc = await Link.GetDocument("https://google.com/");
            IList<HtmlNode> title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "Google" == item.InnerHtml);
        }

        /// <summary>
        /// Gets the document valid markup.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetDocument_ValidMarkup()
        {
            HtmlDocument doc = await Link.GetDocument(ValidMarkup);
            IList<HtmlNode> title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "groﬂartig" == item.InnerHtml);
        }

        /// <summary>
        /// Gets the document invalid string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData(BrokenMarkup)]
        [InlineData("")]
        public async Task GetDocument_InvalidString(string input)
        {
            HtmlDocument doc = await Link.GetDocument(input);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the image url for a  dilbert comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_Single_Dilbert()
        {
            Link link = new Link();

            Assert.Contains(
                "http://assets.amuniversal.com/64a5e1b036e9012ea5cb00163e41dd5b", 
                await link.GetImageUrls("http://dilbert.com/strip/2011-03-24"));
        }

        /// <summary>
        /// Gets the image urls for a multi-panel schisslaweng comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_Multiple_Schisslaweng()
        {
            Link link = new Link();

            IList<string> foundImageUrls = await link.GetImageUrls("https://www.schisslaweng.net/probe/");
            IList<string> expectedImageUrls = new List<string>
            {
                "https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/01_Trainingistalles_FINAL_web-980x1386.jpg",
                "https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/02_Trainingistalles_FINAL_web-980x1386.jpg",
                "https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/03_Trainingistalles_FINAL_web-980x1386.jpg"
            };

            Assert.Equal(
                expectedImageUrls, 
                foundImageUrls);
        }

        /// <summary>
        /// Gets the image urls for an invalid url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetImageUrls_Invalid(string url)
        {
            Link link = new Link();

            Assert.Null(await link.GetImageUrls(url));
        }

        /// <summary>
        /// Gets the image urls from a non configured url.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_NonConfigured()
        {
            Link link = new Link();

            Assert.Null(await link.GetImageUrls("http://this.pageis.not/present"));
        }
        
        /// <summary>
        /// Gets the image file name for a  dilbert comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageFileName_Dilbert()
        {
            Link link = new Link();

            Assert.Contains(
                "2018-01-23_-_User Specifications Are Not Complete",
                await link.GetImageFileName("http://dilbert.com/strip/2018-01-23"));
        }

        /// <summary>
        /// Gets the image file name for an invalid url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetImageFileName_Invalid(string url)
        {
            Link link = new Link();

            Assert.Null(await link.GetImageFileName(url));
        }

        /// <summary>
        /// Gets the image file name from a non configured url.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageFileName_NonConfigured()
        {
            Link link = new Link();

            Assert.Null(await link.GetImageFileName("http://this.pageis.not/present"));
        }
    }
}