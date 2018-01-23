﻿// <copyright file="HttpRequester.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetThatPic.Data.IO
{
    /// <summary>
    /// Performs Http Requests
    /// </summary>
    public class HttpRequester
    {
        /// <summary>
        /// The protocol and domain finding Regex.
        /// </summary>
        public static readonly Regex ProtocolAndDomain = new Regex("^(https?://.*?)/.*$");

        /// <summary>
        /// The path afterdomain finding Regex.
        /// </summary>
        public static readonly Regex PathAfterdomain = new Regex("^https?://.*?(/.*)$");

        /// <summary>
        /// Gets a string response.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The response.</returns>
        public async Task<string> GetString(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !ProtocolAndDomain.IsMatch(url))
            {
                return null;
            }

            try
            {
                var client = new HttpClient();
                var stringTask = client.GetStringAsync(url);
                string response = await stringTask;
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
