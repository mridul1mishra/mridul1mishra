using AD.Youtube.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace AD.Youtube
{
    public static class Utility
    {
        public static Search GetLatestVideo(string apiKey, string channelID)
        {
            return Search(apiKey, channelID);
        }

        private static Search Search(string apiKey, string channelID)
        {
            try
            {
                string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1&order=date&type=video&key={apiKey}&channelId={channelID}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                WebResponse response = request.GetResponse();

                string json;

                using (response)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();
                    }
                }

                return JsonConvert.DeserializeObject<Search>(json);
            }
            catch (Exception){}

            return null;
        }
    }
}
