// <copyright file="DomainsJsonTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // TODO: All testst starting with Gamercat.

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
        /// Checks if the file names get correctly parsed for: dilbert
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("http://dilbert.com/strip/2011-03-24", "2011-03-24")]
        [InlineData("http://dilbert.com/strip/2015-01-05", "2015-01-05_-_Dating_Is_A_B_Testing")]
        public async Task Dilbert_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for: dilbert
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("http://dilbert.com/strip/2011-03-24", "http://assets.amuniversal.com/64a5e1b036e9012ea5cb00163e41dd5b")]
        public async Task Dilbert_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
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
        /// Checks if the file names get correctly parsed for: Oh joy sex toy
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("https://www.ohjoysextoy.com/hitachi/", "2013-04-30_-_Hitachi_Magic_Wand")]
        public async Task OhJoy_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for:  Oh joy sex toy
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("https://www.ohjoysextoy.com/hitachi/", "https://www.ohjoysextoy.com/comics/2013-04-30-hitachi.jpg")]
        public async Task OhJoy_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
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
        /// Checks if the file names get correctly parsed for: turnoff.us
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("http://turnoff.us/geek/software-test/", "Software_Testing")]
        public async Task TurnoffUs_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for:  turnoff.us
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("http://turnoff.us/geek/software-test/", "http://turnoff.us/image/en/test.png")]
        public async Task TurnoffUs_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: thecodinglove.com
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
        /// Checks if the file names get correctly parsed for: thecodinglove.com
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing", "When_I_commit_without_testing")]
        public async Task TheCodingLove_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for:  thecodinglove.com
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing", "https://ljdchost.com/hVkEriQ.gif")]
        public async Task TheCodingLove_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: xkcd
        /// </summary>
        /// <param name="url">The URL.</param>
        [Theory]
        [InlineData("https://xkcd.com/681/")]
        public void Xkcd_Name(string url)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal("xkcd.com", domain.Name);
        }

        /// <summary>
        /// Checks if all image sizes get parsed correctly for: xkcd
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
        /// Checks if the file names get correctly parsed for: xkcd
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("https://xkcd.com/1513/", "1513_-_Code_Quality_-_I_honestly_did_not_think_you_could_even_USE_emoji_in_variable_names_Or_that_there_were_so_many_different_crying_ones")]
        [InlineData("https://xkcd.com/1518/", "1518_-_Typical_Morning_Routine_-_Hang_on_I_have_heard_this_problem_We_need_to_pour_water_into_the_duct_until_the_phone_floats_up_and_wait_phones_sink_in_water_Mercury_We_need_a_vat_of_mercury_to_pour_down_the_vent_That_will_definitely_make")]
        public async Task Xkcd_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
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
        /// Checks if the file names get correctly parsed for: giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA", "fury_kung_fury_kung_hackerman")]
        public async Task Giphy_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for:  giphy
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA", "https://i.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        public async Task Giphy_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
        }

        /// <summary>
        /// Checks if the domain is correctly identified for all valid domains of: media.giphy.com
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
        /// Checks if the domain is correctly identified for all valid domains of: Schisslaweng
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
        /// Checks if multi-file comics get correctly identified for: Schisslaweng
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

        /// <summary>
        /// Checks if the file names get correctly parsed for: Schisslaweng
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        [Theory]
        [InlineData("https://www.schisslaweng.net/probe/", "2017-03-29_-_Probe")]
        public async Task Schisslaweng_FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed for:  Schisslaweng
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        [Theory]
        [InlineData("https://www.schisslaweng.net/probe/", "https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/02_Trainingistalles_FINAL_web-980x1386.jpg")]
        public async Task Schisslaweng_ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = await link.GetImageUrls(url);
            Assert.Contains(expectedImageUrl, fileName);
        }
    }
}
