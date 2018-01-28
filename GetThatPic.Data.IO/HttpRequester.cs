// <copyright file="HttpRequester.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GetThatPic.Data.IO
{
    /// <summary>
    /// Performs Http Requests
    /// </summary>
    public static class HttpRequester
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
        public static async Task<string> GetString(string url)
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

        /// <summary>
        /// Gets the stream for a given url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A Task.</returns>
        public static async Task<Stream> GetStream(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !ProtocolAndDomain.IsMatch(url))
            {
                return null;
            }

            try
            {
                var client = new HttpClient();
                Stream stream = await client.GetStreamAsync(url);
                return stream;
            }
            catch
            {
                return null;
            }
        }
        
        /// <summary>
        /// Loads the image from URL to preview.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A bitmap of the given url's target image if valid.</returns>
        public static BitmapImage GetImage(string url)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();

                return bitmap;
            }
            catch
            {
                return null;
            }
        }
    }
}
