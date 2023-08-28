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
        <title>gro�artig</title>
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
        private const string BrokenMarkup            = "<html<head><title>gro�artig</title>";

        /// <summary>
        /// Tests the constructor behaviour when passing true.
        /// </summary>
        [Fact]
        public void Constructor_True()
        {
            var link = new Link(true, false);

            Assert.Contains(link.Domains, domain => domain.Url.IsMatch("http://dilbert.com"));
        }

        /// <summary>
        /// Tests the constructor behaviour when passing false.
        /// </summary>
        [Fact]
        public void Constructor_False()
        {
            var link = new Link(false, false);

            Assert.DoesNotContain(link.Domains, domain => domain.Url.IsMatch("http://dilbert.com"));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: empty configuration, non empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_EmptyConfig_NonEmptyUrl()
        {
            var link = new Link(false, false);

            Assert.Null(link.IdentifyDomain("http://dilbert.com"));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: empty configuration, empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_EmptyConfig_EmptyUrl()
        {
            var link = new Link(false, false);

            Assert.Null(link.IdentifyDomain(null));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: non empty configuration, empty URL.
        /// </summary>
        [Fact]
        public void IdentifyDomain_NonEmptyConfig_EmptyUrl()
        {
            var link = new Link(false, false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = new Regex("http://dilbert.com"),
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
            var link = new Link(false, false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = new Regex("http://dilbert.com"),
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });

            Assert.True(link.IdentifyDomain("http://dilbert.com/strip/2018-01-22")?.Url.IsMatch("http://dilbert.com"));
        }

        /// <summary>
        /// Tests the functionality of IdentifyDomain for: non empty configuration, non empty URL, Url not contained.
        /// </summary>
        [Fact]
        public void IdentifyDomain_NonEmptyConfig_NonEmptyUrl_NotContained()
        {
            var link = new Link(false, false);

            link.Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = new Regex("http://dilbert.com"),
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });

            Assert.Null(link.IdentifyDomain("https://google.com")?.Url);
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with no element given.
        /// </summary>
        [Fact]
        public void InitializeConfig_NoElementAdded()
        {
            var link = new Link(false, false);
            link.InitializeConfig(true, false);

            Assert.Contains(link.Domains, domain => domain.Url.IsMatch("http://dilbert.com"));
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_Title()
        {
            var doc = await Link.GetDocumentFromUrl("https://google.com/");
            var title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "Google" == item.InnerHtml);
        }

        /// <summary>
        /// Gets null.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_null()
        {
            var doc = await Link.GetDocumentFromUrl(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_empty()
        {
            var doc = await Link.GetDocumentFromUrl(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Title()
        {
            var doc = Link.GetDocumentFromMarkup(ValidMarkup);
            var title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "gro�artig" == item.InnerHtml);
        }

        /// <summary>
        /// Gets null.
        /// </summary>
        [Fact]
        public  void GetDocumentFromMarkup_null()
        {
            var doc = Link.GetDocumentFromMarkup(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Empty()
        {
            var doc = Link.GetDocumentFromMarkup(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an empty string.
        /// </summary>
        ////[Fact]
        ////public void GetDocumentFromMarkup_Invalid()
        ////{
        ////HtmlDocument doc = Link.GetDocumentFromMarkup(BrokenMarkup);
        ////Assert.Null(doc);
        ////}

        /// <summary>
        /// Gets the document URL.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetDocument_Url()
        {
            var doc = await Link.GetDocument("https://google.com/");
            var title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "Google" == item.InnerHtml);
        }

        /// <summary>
        /// Gets the document valid markup.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetDocument_ValidMarkup()
        {
            var doc = await Link.GetDocument(ValidMarkup);
            var title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "gro�artig" == item.InnerHtml);
        }

        /// <summary>
        /// Gets the document invalid string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A Task.</returns>
        [Theory]

        // Deactivated - this is now allowed.
        ////[InlineData(BrokenMarkup)]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetDocument_InvalidString(string input)
        {
            var doc = await Link.GetDocument(input);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the image url for a  dilbert comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_Single_Dilbert()
        {
            var link = new Link(true, false);

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
            var link = new Link(true, false);

            var foundImageUrls = await link.GetImageUrls("https://www.schisslaweng.net/probe/");
            IEnumerable<string> expectedImageUrls = new List<string>
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
            var link = new Link(true, false);

            Assert.Empty(await link.GetImageUrls(url));
        }

        /// <summary>
        /// Gets the image urls from a non configured url.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_NonConfigured()
        {
            var link = new Link(true, false);

            Assert.Empty(await link.GetImageUrls("http://this.pageis.not/present"));
        }
        
        /// <summary>
        /// Gets the image file name for a  dilbert comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageFileName_Dilbert()
        {
            var link = new Link(true, false);

            var output = await link.GetImageFileName("http://dilbert.com/strip/2018-01-23");
            Assert.Contains(
                "2018-01-23_-_User_Specifications_Are_Not_Complete",
                output);
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
            var link = new Link(true, false);

            Assert.Empty(await link.GetImageFileName(url));
        }

        /// <summary>
        /// Gets the image file name from a non configured url.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageFileName_NonConfigured()
        {
            var link = new Link(true, false);

            Assert.Empty(await link.GetImageFileName("http://this.pageis.not/present"));
        }

        /// <summary>
        /// Validates correft file name extraction when a single file ending is present.
        /// </summary>
        [Fact]
        public void FileEndingFromUrl_SingleDot()
        {
            Assert.Equal(".jpg", Link.FileEndingFromUrl("https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/01_Trainingistalles_FINAL_web-980x1386.jpg"));
        }

        /// <summary>
        /// Validates correft file name extraction when a double file ending is present (should only get the last one).
        /// </summary>
        [Fact]
        public void FileEndingFromUrl_DoubleDot()
        {
            Assert.Equal(".gif", Link.FileEndingFromUrl("https://ljdchost.com/DgKiUtL.big.gif"));
        }

        /// <summary>
        /// Validates correft file name extraction when no file ending is present.
        /// </summary>
        [Fact]
        public void FileEndingFromUrl_NoDot()
        {
            Assert.Equal(string.Empty, Link.FileEndingFromUrl("http://assets.amuniversal.com/aa0c23f0d6c801350cc9005056a9545d"));
        }

        /// <summary>
        /// Validates correft file name extraction when the input is empty.
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FileEndingFromUrl_NoUrl(string url)
        {
            Assert.Equal(string.Empty, Link.FileEndingFromUrl(url));
        }
    }
}