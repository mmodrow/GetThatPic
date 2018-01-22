// <copyright file="LinkTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Linq;
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
        /// Tests the constructor behaviour when passing true.
        /// </summary>
        [Fact]
        public void Constructor_True()
        {
            Link link = new Link(true);

            Assert.True(link.Domains.Any(domain => "http://dilbert.com" == domain.Url));
        }

        /// <summary>
        /// Tests the constructor behaviour when passing false.
        /// </summary>
        [Fact]
        public void Constructor_False()
        {
            Link link = new Link(false);

            Assert.False(link.Domains.Any(domain => "http://dilbert.com" == domain.Url));
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

            Assert.Equal(null, link.IdentifyDomain("https://google.com")?.Url);
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

            Assert.True(link.Domains.Any(domain => "http://dilbert.com" == domain.Url));
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with no element given.
        /// </summary>
        [Fact]
        public void InitializeConfig_NoElementAdded()
        {
            Link link = new Link(false);
            link.InitializeConfig();

            Assert.True(link.Domains.Any(domain => "http://dilbert.com" == domain.Url));
        }

        /// <summary>
        /// Tests the functionality of InitializeConfig with an empty list given.
        /// </summary>
        [Fact]
        public void InitializeConfig_EmptyListAdded()
        {
            Link link = new Link(false);
            link.InitializeConfig(true, new List<Domain>());

            Assert.False(link.Domains.Any(domain => "http://dilbert.com" == domain.Url));
        }
    }
}
