using System.IO;
using System.Net;

namespace AD.TwitterTools
{
    class Utility
    {
        public string RequstJson(string apiUrl, string tokenType, string accessToken)
        {
            var json = string.Empty;
            HttpWebRequest apiRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            var timelineHeaderFormat = "{0} {1}";
            apiRequest.Headers.Add("Authorization",
                                        string.Format(timelineHeaderFormat, tokenType,
                                                      accessToken));
            apiRequest.Method = "Get";
            WebResponse timeLineResponse = apiRequest.GetResponse();

            using (timeLineResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                    // The below can be used to deserialize into a c# object
                    //var result = JsonConvert.DeserializeObject<List<TimeLine>>(json);
                }
            }
            return json;
        }
    }
}
