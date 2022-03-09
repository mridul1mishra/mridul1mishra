using AD.LinkedIn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AD.LinkedIn
{
    public static class Utility
    {
        public static string GetAuthorizationCode(string clientID, string redirectUrl)
        {
            string url = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&scope=rw_company_admin&client_id={clientID}&redirect_uri={redirectUrl}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            WebResponse response = request.GetResponse();
            string location = response.Headers["location"];

            return !string.IsNullOrEmpty(location) ? location : string.Empty;
        }

        public static string GetAccessToken(string clientID, string clientSecret, string redirectUrl)
        {
            string authorizationCode = GetAuthorizationCode(clientID, redirectUrl);

            string url = $"https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code&code={authorizationCode}&client_id={clientID}&client_secret={clientSecret}&redirect_uri=http%3A%2F%2Flocalhost";
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

            var result = JsonConvert.DeserializeObject<AccessToken>(json);

            return result!=null ? result.access_token : string.Empty;
        }

        public static string GetLatestUpdate(string companyID, string clientID, string clientSecret, string redirectUrl)
        {
            string accessToken = GetAccessToken(clientID, clientSecret, redirectUrl);

            string url = $"https://api.linkedin.com/v1/companies/{companyID}/updates?format=json&oauth2_access_token={accessToken}";
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

            var result = JsonConvert.DeserializeObject<Updates>(json);

            var update = result.values.FirstOrDefault();
            if(update!=null)
            {
                return update.updateContent.companyStatusUpdate.share.comment;
            }
            return string.Empty;
        }


    }
}
