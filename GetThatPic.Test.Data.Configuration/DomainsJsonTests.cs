// <copyright file="DomainsJsonTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using GetThatPic.Data.Configuration;
using GetThatPic.Parsing;
using Xunit;

namespace GetThatPic.Test.Data.Configuration
{
    /// <summary>
    /// Tests if all the configured domains in Domains.json are still OK.
    /// These tests should fail as soon as someone changes their site in incompatible ways.
    /// </summary>
    public class DomainsJsonTests
    {
        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: dilbert
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("http://dilbert.com/strip/2018-01-23")]
        public void dilbert_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("dilbert.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: oh joy sex toy
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://www.ohjoysextoy.com/introduction/")]
        public void ohJoy_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("ohjoysextoy.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: turnoff.us
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("http://turnoff.us/geek/software-test/")]
        public void turnoffUs_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("turnoff.us", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: turnoff.us
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing")]
        public void theCodingLove_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("thecodinglove.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA")]
        public void giphy_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("giphy.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: 
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://media.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        [InlineData("https://media1.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        [InlineData("https://media5.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        [InlineData("https://media12.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        public void mediaGiphy_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("media.giphy.com", domain?.Name);
        }
    }
}
