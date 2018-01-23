// <copyright file="HttpRequesterTests.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System;
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
    }
}
