// <copyright file="LinkTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Text.RegularExpressions;
using GetThatPic.Data.Configuration;
using GetThatPic.Parsing;
using Xunit;

namespace GetThatPic.Test.Parsing
{
    /// <summary>
    /// Tests functionality of the Link class.
    /// </summary>
    public class LinkTests
    {
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

            Assert.Equal("http://dilbert.com", link.IdentifyDomain("http://dilbert.com")?.Url);
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

            Assert.Equal(null, link.IdentifyDomain("https://google.com")?.Url);
        }
    }
}
