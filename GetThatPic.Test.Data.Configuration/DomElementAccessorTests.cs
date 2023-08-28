// <copyright file="DomElementAccessorTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetThatPic.Data.Configuration;
using GetThatPic.Parsing;
using HtmlAgilityPack;
using Xunit;

namespace GetThatPic.Test.Data.Configuration
{
    /// <summary>
    /// Tests the DomElementAccessor.
    /// </summary>
    public class DomElementAccessorTests
    {
        /// <summary>
        /// The valid markup example.
        /// </summary>
        private const string ValidMarkup = @"<html>
    <head>
        <title>großartig</title>
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
        private const string BrokenMarkup = "<html<head><title>großartig</title>";
        
        /// <summary>
        /// Tries GetContent with an invalid selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_InvalidSelector(string selector)
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Text,
                Selector = selector
            };

            var content = accessor.GetContent(doc);

            Assert.Empty(content);
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
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                Selector = "span",
                AttributeName = name
            };

            var content = accessor.GetContent(doc);

            Assert.Empty(content);
        }

        /// <summary>
        /// Gets the content inner text.
        /// </summary>
        [Fact]
        public void GetContent_InnerText()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Text,
                Selector = "head"
            };

            var content = accessor.GetContent(doc).FirstOrDefault()?.Trim();

            Assert.Equal("großartig", content);
        }

        /// <summary>
        /// Gets the content inner HTML single.
        /// </summary>
        [Fact]
        public void GetContent_InnerHtml_Single()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Html,
                Selector = "head"
            };

            var content = accessor.GetContent(doc).FirstOrDefault();

            Assert.Equal(
@"
        <title>großartig</title>
    ",
                content);
        }

        /// <summary>
        /// Gets the content inner HTML multiple.
        /// </summary>
        [Fact]
        public void GetContent_InnerHtml_Multiple()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Html,
                Selector = "h2"
            };

            var content = accessor.GetContent(doc);

            Assert.Equal(2, content.Count);
        }

        /// <summary>
        /// Gets the content attribute data.
        /// </summary>
        [Fact]
        public void GetContent_Attribute_Data()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild"
            };

            var content = accessor.GetContent(doc).FirstOrDefault();

            Assert.Equal("Dreams are made of", content);
        }

        /// <summary>
        /// Gets the content and replaces it.
        /// </summary>
        [Fact]
        public void GetContent_Pattern_Valid()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = new Regex(@"^((?:[A-Za-z]+\s){3}).*$"),
                Replace = "$1up"
            };

            var content = accessor.GetContent(doc).FirstOrDefault();

            Assert.Equal("Dreams are made up", content);
        }

        /// <summary>
        /// Gets the content andtries to replace it with an invalid regex.
        /// </summary>
        /// <param name="regexContent">ImageUrl of the regex.</param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetContent_Pattern_Invalid(string regexContent)
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = null != regexContent ? new Regex(regexContent) : null
            };

            var content = accessor.GetContent(doc)?.FirstOrDefault();

            Assert.Null(content);
        }

        /// <summary>
        /// Gets the content and replaces it.
        /// </summary>
        [Fact]
        public void GetContent_Replace_Valid()
        {
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Pattern = new Regex(@"^((?:[A-Za-z]+\s){3}).*$")
            };

            var content = accessor.GetContent(doc).FirstOrDefault();

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
            var doc = UrlParser.GetDocumentFromMarkup(ValidMarkup);
            var accessor = new DomElementAccessor()
            {
                Type = DomElementAccessor.TargetType.Attribute,
                AttributeName = "data-stuff",
                Selector = "#MainNavChild",
                Replace = replace
            };

            var content = accessor.GetContent(doc)?.FirstOrDefault();

            Assert.Equal(replace, content);
        }
    }
}
