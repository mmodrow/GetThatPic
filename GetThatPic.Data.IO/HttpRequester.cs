// <copyright file="HttpRequester.cs" company="Marc A. Modrow">
// Copyright (c) 2018 All Rights Reserved
// <author>Marc A. Modrow</author>
// </copyright>

using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetThatPic.Data.IO;

/// <summary>
/// Performs Http Requests
/// </summary>
public static class HttpRequester
{
    /// <summary>
    /// The protocol and domain finding Regex.
    /// </summary>
    public static readonly Regex ProtocolAndDomain = new("^(https?://.*?)/.*$");

    /// <summary>
    /// The path afterdomain finding Regex.
    /// </summary>
    public static readonly Regex PathAfterDomain = new("^https?://.*?(/.*)$");

    /// <summary>
    /// The HTTP de-zip handler.
    /// </summary>
    private static readonly HttpClientHandler Handler = new()
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    };

    /// <summary>
    /// The HTTP client.
    /// </summary>
    private static readonly HttpClient Client = new(Handler);

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
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
            return await Client.GetStringAsync(url);
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
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
            var stream = await Client.GetStreamAsync(url);
            return stream;
        }
        catch
        {
            ////(Exception e)
            // TODO: Log e.Message.
            return null;
        }
    }
}