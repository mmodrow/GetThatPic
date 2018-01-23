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
        [Fact]
        public async Task Get_NullAsync()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.Get(null));
        }

        /// <summary>
        /// Gets an empty url.
        /// </summary>
        [Fact]
        public async Task Get_Empty()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.Get(String.Empty));
        }

        /// <summary>
        /// Gets a non url.
        /// </summary>
        [Fact]
        public async Task Get_NonUrl()
        {
            HttpRequester requester = new HttpRequester();
            Assert.Null(await requester.Get("this is no path"));
        }

        /// <summary>
        /// Gets the google.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_Google()
        {
            HttpRequester requester = new HttpRequester();
            string response = await requester.Get("https://google.com/");
            Assert.Contains("<title>Google</title>", response);
        }
    }
}
