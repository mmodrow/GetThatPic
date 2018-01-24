// <copyright file="LinkTests.cs" company="Marc A. Modrow">
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
            Link link = new Link();
            HtmlDocument doc = await link.GetDocumentFromUrl("https://google.com/");
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
            Link link = new Link();
            HtmlDocument doc = await link.GetDocumentFromUrl(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetDocumentFromUrl_empty()
        {
            Link link = new Link();
            HtmlDocument doc = await link.GetDocumentFromUrl(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Title()
        {
            Link link = new Link();
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            IList<HtmlNode> title = doc.QuerySelectorAll("title");
            Assert.Contains(title, item => "groﬂartig" == item.InnerHtml);
        }

        /// <summary>
        /// Gets null.
        /// </summary>
        [Fact]
        public  void GetDocumentFromMarkup_null()
        {
            Link link = new Link();
            HtmlDocument doc = link.GetDocumentFromMarkup(null);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an epty string.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Empty()
        {
            Link link = new Link();
            HtmlDocument doc = link.GetDocumentFromMarkup(string.Empty);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets an empty string.
        /// </summary>
        [Fact]
        public void GetDocumentFromMarkup_Invalid()
        {
            Link link = new Link();
            HtmlDocument doc = link.GetDocumentFromMarkup(BrokenMarkup);
            Assert.Null(doc);
        }

        /// <summary>
        /// Gets the document URL.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetDocument_Url()
        {
            Link link = new Link();
            HtmlDocument doc = await link.GetDocument("https://google.com/");
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
            Link link = new Link();
            HtmlDocument doc = await link.GetDocument(ValidMarkup);
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
            Link link = new Link();
            HtmlDocument doc = await link.GetDocument(input);
            Assert.Null(doc);
        }

        /// <summary>
        /// Tries GetContent with a null accessor
        /// </summary>
        [Fact]
        public void GetContent_nullAccessor()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = null;

            IList<string> content = link.GetContent(doc, accessor);

            Assert.Null(content);
        }

        /// <summary>
        /// Tries GetContent with an invalid selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_InvalidSelector(string selector)
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Text,
                Selector = selector
            };

            IList<string> content = link.GetContent(doc, accessor);

            Assert.Null(content);
        }

        /// <summary>
        /// Tries GetContent with an invalid selector.
        /// </summary>
        /// <param name="name">The name.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_InvalidAttributeName(string name)
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                Selector = "span",
                AttributeName = name
            };

            IList<string> content = link.GetContent(doc, accessor);

            Assert.Null(content);
        }

        /// <summary>
        /// Gets the content inner text.
        /// </summary>
        [Fact]
        public void GetContent_InnerText()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Text,
                Selector = "head"
            };

            string content = link.GetContent(doc, accessor).FirstOrDefault()?.Trim();

            Assert.Equal("groﬂartig", content);
        }

        /// <summary>
        /// Gets the content inner HTML single.
        /// </summary>
        [Fact]
        public void GetContent_InnerHtml_Single()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Html,
                Selector = "head"
            };

            string content = link.GetContent(doc, accessor).FirstOrDefault();

            Assert.Equal(
@"
        <title>groﬂartig</title>
    ", 
                content);
        }

        /// <summary>
        /// Gets the content inner HTML multiple.
        /// </summary>
        [Fact]
        public void GetContent_InnerHtml_Multiple()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Html,
                Selector = "h2"
            };

            IList<string> content = link.GetContent(doc, accessor);

            Assert.Equal(2, content.Count);
        }

        /// <summary>
        /// Gets the content attribute data.
        /// </summary>
        [Fact]
        public void GetContent_Attribute_Data()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild"
            };

            string content = link.GetContent(doc, accessor).FirstOrDefault();

            Assert.Equal("Dreams are made of", content);
        }

        /// <summary>
        /// Gets the content and replaces it.
        /// </summary>
        [Fact]
        public void GetContent_Pattern_Valid()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = new Regex(@"^((?:[A-Za-z]+\s){3}).*$"),
                Replace = "$1up"
            };

            string content = link.GetContent(doc, accessor).FirstOrDefault();

            Assert.Equal("Dreams are made up", content);
        }

        /// <summary>
        /// Gets the content andtries to replace it with an invalid regex.
        /// </summary>
        /// <param name="regexContent">Content of the regex.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_Pattern_Invalid(string regexContent)
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = null != regexContent ? new Regex(regexContent) : null
            };

            string content = link.GetContent(doc, accessor)?.FirstOrDefault();

            Assert.Null(content);
        }

        /// <summary>
        /// Gets the content and replaces it.
        /// </summary>
        [Fact]
        public void GetContent_Replace_Valid()
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = new Regex(@"^((?:[A-Za-z]+\s){3}).*$")
            };

            string content = link.GetContent(doc, accessor).FirstOrDefault();

            Assert.Equal("Dreams are made ", content);
        }

        /// <summary>
        /// Gets the content andtries to replace it with an invalid replacement string.
        /// </summary>
        /// <param name="replace">The replace.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_Replace_Invalid(string replace)
        {
            Link link = new Link(false);
            HtmlDocument doc = link.GetDocumentFromMarkup(ValidMarkup);
            DomElementAccessor accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Replace = replace
            };

            string content = link.GetContent(doc, accessor)?.FirstOrDefault();

            Assert.Null(content);
        }

        /// <summary>
        /// Gets the image urls for a  dilbert comic.
        /// </summary>
        /// <returns>A Task.</returns>
        [Fact]
        public async Task GetImageUrls_Dilbert()
        {
            Link link = new Link();

            Assert.Contains(
                "http://assets.amuniversal.com/64a5e1b036e9012ea5cb00163e41dd5b", 
                await link.GetImageUrls("http://dilbert.com/strip/2011-03-24"));
        }
    }
}