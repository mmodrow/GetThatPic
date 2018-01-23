using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetThatPic.Data.IO
{
    public class HttpRequester
    {
        public static readonly Regex ProtocolAndDomain = new Regex("^(https?://.*?)/.*$");
        public static readonly Regex PathAfterdomain = new Regex("^https?://.*?(/.*)$");

        public async Task<string> Get(string url)
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
