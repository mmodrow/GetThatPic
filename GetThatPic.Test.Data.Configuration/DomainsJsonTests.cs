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
        /// Checks if the domain is correctly identified for all valid domains.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The expected name.</param>
        [Theory]
        [InlineData("http://dilbert.com/strip/2018-01-23", "dilbert.com")]
        [InlineData("https://www.ohjoysextoy.com/introduction/", "ohjoysextoy.com")]
        [InlineData("http://turnoff.us/geek/software-test/", "turnoff.us")]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing", "thecodinglove.com")]
        [InlineData("https://xkcd.com/681/", "xkcd.com")]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA", "giphy.com")]
        [InlineData("https://media.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "media.giphy.com")]
        [InlineData("https://media1.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "media.giphy.com")]
        [InlineData("https://media5.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "media.giphy.com")]
        [InlineData("https://media12.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "media.giphy.com")]
        [InlineData("https://www.schisslaweng.net/probe/", "Schisslaweng")]
        [InlineData("http://thegamercat.com/comic/fancy-footwork/", "The Gamer Cat")]
        [InlineData("http://awkwardzombie.com/index.php?page=0&comic=011711", "awkwardzombie.com")]
        [InlineData("http://sarahburrini.com/wordpress/comic/protestpirat/", "Das Leben ist kein Ponyhof")]
        [InlineData("http://phdcomics.com/comics/archive.php?comicid=1993", "phdcomics.com")]
        [InlineData("http://explosm.net/comics/4833/", "Cyanide & Happiness")]
        [InlineData("http://static.nichtlustig.de/toondb/150422.html", "nichtlustig.de")]
        [InlineData("http://ruthe.de/cartoon/3145/datum/asc/", "ruthe.de")]
        [InlineData("http://www.totaberlustig.com/erster-cartoonist/", "totaberlustig.com")]
        [InlineData("https://www.marvcomics.com/comics/mittenmang/1515-mittenmang-23", "Marvin Cliffords Mittenmang")]
        [InlineData("http://www.questionablecontent.net/view.php?comic=1324", "questionablecontent.net")]
        [InlineData("http://ars.userfriendly.org/cartoons/?id=20130719", "userfriendly.org")]
        [InlineData("https://www.graphitti-blog.de/2017/11/12/mein-biorhythmus/", "graphitti-blog.de")]
        public void Name(string url, string expectedName)
        {
            Link link = new Link();

            Domain domain = link.IdentifyDomain(url);
            Assert.Equal(expectedName, domain.Name);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedName">The exptected file name.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData("http://dilbert.com/strip/2011-03-24", "2011-03-24")]
        [InlineData("http://dilbert.com/strip/2015-01-05", "2015-01-05_-_Dating_Is_A_B_Testing")]
        [InlineData("https://www.ohjoysextoy.com/hitachi/", "2013-04-30_-_Hitachi_Magic_Wand")]
        [InlineData("http://turnoff.us/geek/software-test/", "Software_Testing")]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing", "When_I_commit_without_testing")]
        [InlineData("https://xkcd.com/1513/", "1513_-_Code_Quality_-_I_honestly_did_not_think_you_could_even_USE_emoji_in_variable_names_Or_that_there_were_so_many_different_crying_ones")]
        [InlineData("https://xkcd.com/1518/", "1518_-_Typical_Morning_Routine_-_Hang_on_I_have_heard_this_problem_We_need_to_pour_water_into_the_duct_until_the_phone_floats_up_and_wait_phones_sink_in_water_Mercury_We_need_a_vat_of_mercury_to_pour_down_the_vent_That_will_definitely_make")]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA", "fury_kung_fury_kung_hackerman")]
        [InlineData("https://www.schisslaweng.net/probe/", "2017-03-29_-_Probe")]
        [InlineData("http://thegamercat.com/comic/fancy-footwork/", "260_-_2017-12-04_-_Fancy_Footwork_-_Skillz_that_pay_the_billz")]
        [InlineData("http://awkwardzombie.com/index.php?page=0&comic=011711", "2011-01-17_-_Recettear_an_Item_Shop_s_Tale_-_To_The_Victims_Go_The_Spoileds")]
        [InlineData("http://sarahburrini.com/wordpress/comic/protestpirat/", "2012-01-26_-_Protestpirat")]
        [InlineData("http://phdcomics.com/comics/archive.php?comicid=1993", "2018-01-22_-_Psych")]
        [InlineData("http://explosm.net/comics/4833/", "4833_-_2018-01-21_-_Kris_Wilson_-_lifespan")]
        [InlineData("http://static.nichtlustig.de/toondb/150422.html", "2015-04-22_-_MOERDER")]
        [InlineData("http://ruthe.de/cartoon/2232/datum/asc/", "1563")]
        [InlineData("http://www.totaberlustig.com/erster-cartoonist/", "2017-01-17_-_Michael_-_Erster_Cartoonist")]
        [InlineData("https://www.marvcomics.com/comics/mittenmang/1515-mittenmang-23", "23_-_2017-08-21_-_Vergangenheit")]
        [InlineData("http://www.questionablecontent.net/view.php?comic=1324", "1324")]
        [InlineData("http://ars.userfriendly.org/cartoons/?id=20130719", "2013-07-19")]
        [InlineData("https://www.graphitti-blog.de/2017/11/12/mein-biorhythmus/", "2017-11-12_-_Mein_Biorhythmus")]

        // Currently this is broken in some way.
        ////[InlineData("https://media1.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "fury_kung_fury_kung_hackerman")]
        public async Task FileName(string url, string expectedName)
        {
            Link link = new Link();

            string fileName = await link.GetImageFileName(url);
            Assert.Equal(expectedName, fileName);
        }

        /// <summary>
        /// Checks if the file names get correctly parsed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="expectedImageUrl">The expected Image Url.</param>
        /// <returns>A Task.</returns>
        [Theory]
        [InlineData("http://dilbert.com/strip/2011-03-24", "http://assets.amuniversal.com/64a5e1b036e9012ea5cb00163e41dd5b")]
        [InlineData("https://www.ohjoysextoy.com/hitachi/", "https://www.ohjoysextoy.com/comics/2013-04-30-hitachi.jpg")]
        [InlineData("http://turnoff.us/geek/software-test/", "http://turnoff.us/image/en/test.png")]
        [InlineData("http://thecodinglove.com/post/170032454677/when-i-commit-without-testing", "https://ljdchost.com/hVkEriQ.gif")]
        [InlineData("https://xkcd.com/681/", "https://imgs.xkcd.com/comics/gravity_wells_large.png")]
        [InlineData("https://xkcd.com/1513/", "https://imgs.xkcd.com/comics/code_quality.png")]
        [InlineData("https://giphy.com/gifs/fury-kung-hackerman-QbumCX9HFFDQA", "https://i.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        [InlineData("https://www.schisslaweng.net/probe/", "https://www.schisslaweng.net/wp-content/uploads/sites/2/2017/03/02_Trainingistalles_FINAL_web-980x1386.jpg")]
        [InlineData("http://thegamercat.com/comic/fancy-footwork/", "http://thegamercat.com/wp-content/uploads/2017/11/gamercat_260.jpg")]
        [InlineData("http://awkwardzombie.com/index.php?page=0&comic=011711", "http://i49.photobucket.com/albums/f278/katietiedrich/comic182.png")]
        [InlineData("http://sarahburrini.com/wordpress/comic/protestpirat/", "http://sarahburrini.com/wordpress/wp-content/uploads/2012/01/2012-01-26-protestpirat.png")]
        [InlineData("http://phdcomics.com/comics/archive.php?comicid=1993", "http://phdcomics.com/comics/archive/phd012218s.gif")]
        [InlineData("http://explosm.net/comics/4833/", "http://files.explosm.net/comics/Kris/lifespan.png")]
        [InlineData("http://static.nichtlustig.de/toondb/150422.html", "http://static.nichtlustig.de/comics/full/150422.jpg")]
        [InlineData("http://ruthe.de/cartoon/2232/datum/asc/", "http://ruthe.de/cartoons/strip_1563.jpg")]
        [InlineData("http://www.totaberlustig.com/erster-cartoonist/", "http://www.totaberlustig.com/comics/2017-01-17-Erster Cartoonist.jpg")]
        [InlineData("https://www.marvcomics.com/comics/mittenmang/1515-mittenmang-23", "https://www.marvcomics.com/wp-content/uploads/sites/3/2017/08/023_Vergangenheit_web.jpg")]
        [InlineData("http://www.questionablecontent.net/view.php?comic=1324", "http://www.questionablecontent.net/comics/1324.png")]
        [InlineData("http://ars.userfriendly.org/cartoons/?id=20130719", "http://www.userfriendly.org/cartoons/archives/13jul/uf004449.gif")]
        [InlineData("https://www.graphitti-blog.de/2017/11/12/mein-biorhythmus/", "http://www.graphitti-blog.de/wp-content/uploads/2017/11/179_Biorhythmus_Kreis-01.jpg")]
        [InlineData("https://www.graphitti-blog.de/2018/01/04/wahlperiode/", "http://www.graphitti-blog.de/wp-content/uploads/2018/01/Wahlperiode-300x274.png")]

        // Currently this is broken in some way.
        ////[InlineData("https://media1.giphy.com/media/QbumCX9HFFDQA/giphy.gif", "https://i.giphy.com/media/QbumCX9HFFDQA/giphy.gif")]
        public async Task ImageUrl(string url, string expectedImageUrl)
        {
            Link link = new Link();

            IEnumerable<string> fileName = (await link.GetImageUrls(url)).ToList();
            Assert.Contains(expectedImageUrl, fileName);
        }

        /// <summary>
        /// Checks if multi-file comics get correctly identified for: Schisslaweng
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="imageCount">Number of expected images.</param>
        /// <returns>
        /// A Task.
        /// </returns>
        [Theory]
        [InlineData("https://www.schisslaweng.net/probe/", 3)]
        [InlineData("https://www.schisslaweng.net/koerperklaus/", 1)]
        public async Task ImageCount(string url, int imageCount)
        {
            Link link = new Link();

            IEnumerable<string> urls = await link.GetImageUrls(url);
            Assert.Equal(imageCount, urls?.Count());
        }
    }
}
