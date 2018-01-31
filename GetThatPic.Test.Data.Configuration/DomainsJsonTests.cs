// <copyright file="DomainsJsonTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetThatPic.Data.Configuration;
using GetThatPic.Parsing;
using HtmlAgilityPack;
using Xunit;

namespace GetThatPic.Test.Data.Configuration
{
    /// <summary>
    /// Tests if all the configured domains in Domains.json are still OK.
    /// These tests should fail as soon as someone changes their site in incompatible ways.
    /// </summary>
    public class DomainsJsonTests
    {
        // TODO: All testst starting with Gamercat.
        // TODO: File name tests for everything.

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: dilbert
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("http://dilbert.com/strip/2018-01-23")]
        public void Dilbert_Name(string url)
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
        public void OhJoy_Name(string url)
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
        public void TurnoffUs_Name(string url)
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
        public void TheCodingLove_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("thecodinglove.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: xkcd
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://xkcd.com/681/")]
        [InlineData("http://xkcd.com/681/")]
        public void Xkcd_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("xkcd.com", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: xkcd
        /// </summary>
        /// <param name="dropUrl">The drop URL.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData("https://xkcd.com/681/", "https://imgs.xkcd.com/comics/gravity_wells_large.png")]
        [InlineData("https://xkcd.com/1513/", "https://imgs.xkcd.com/comics/code_quality.png")]
        public async Task Xkcd_ImageSizes(string dropUrl, string imageUrl)
        {
            Link link = new Link();
            IEnumerable<string> urls = await link.GetImageUrls(dropUrl);
            Assert.Contains(imageUrl, urls);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA")]
        public void Giphy_Name(string url)
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
        public void MediaGiphy_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("media.giphy.com", domain?.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://www.schisslaweng.net/probe/")]
        public void Schisslaweng_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("Schisslaweng", domain.Name);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="imageCount">Number of expected images.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData("https://www.schisslaweng.net/probe/", 3)]
        [InlineData("https://www.schisslaweng.net/koerperklaus/", 1)]
        public async Task Schisslaweng_Multiple(string url, int imageCount)
        {
            Link link = new Link();

            IEnumerable<string> urls = await link.GetImageUrls(url);
            Assert.Equal(imageCount, urls?.Count());
        }
    }
}
