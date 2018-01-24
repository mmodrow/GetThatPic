// <copyright file="HttpRequesterTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
using System.IO;
using System.Threading.Tasks;
using GetThatPic.Data.IO;
using Xunit;

namespace GetThatPic.Test.Data.IO
{
    /// <summary>
    /// Tests the HttpRequester Functionality.
    /// </summary>
    public class HttpRequesterTests
    {
        /// <summary>
        /// Gets a null url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetString_NullAsync()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetString(null));
        }

        /// <summary>
        /// Gets an empty url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetString_Empty()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetString(string.Empty));
        }

        /// <summary>
        /// Gets a non url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetString_NonUrl()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetString("this is no path"));
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetString_Google()
        {
            HttpRequester requester = new HttpRequester();
            string response = await requester.GetString("https://google.com/");
            Assert.Contains("<title>Google</title>", response);
        }

        /// <summary>
        /// Gets an non-existing url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetString_404()
        {
            HttpRequester requester = new HttpRequester();
            string response = await requester.GetString("https://sgosdbodgosdgosdgosdgsg.gosgsdgogle.coklösdgklöjm/");
            Assert.Null(response);
        }

        /// <summary>
        /// Gets a null url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetStream_NullAsync()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetStream(null));
        }

        /// <summary>
        /// Gets an empty url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetStream_Empty()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetStream(string.Empty));
        }

        /// <summary>
        /// Gets a non url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetStream_NonUrl()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.GetStream("this is no path"));
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetStream_Google()
        {
            HttpRequester requester = new HttpRequester();
            Stream stream = await requester.GetStream("https://google.com/");
            StreamReader reader = new StreamReader(stream);
            Assert.Contains("<title>Google</title>", reader.ReadToEnd());
        }

        /// <summary>
        /// Gets an non-existing url.
        /// </summary>
        /// <returns>A Task for async.</returns>
        [Fact]
        public async Task GetStream_404()
        {
            HttpRequester requester = new HttpRequester();
            Stream response = await requester.GetStream("https://sgosdbodgosdgosdgosdgsg.gosgsdgogle.coklösdgklöjm/");
            Assert.Null(response);
        }
    }
}
