using AD.FacebookTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AD.FacebookTools
{
    public class FacebookClient 
    {
        private readonly HttpClient _httpClient;
        private string BaseAddress = "https://graph.facebook.com";
        public AccessToken AccessToken { get; set; }
        private string ClientID;
        private string ClientSecret;

        public FacebookClient(string clientID, string clientSecret)
        {
            ClientID = clientID;
            ClientSecret = clientSecret;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress)
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            AccessToken = GetAccessToken();
        }

        public AccessToken GetAccessToken()
        {
            var result = GetAsync<AccessToken>($"/v2.12/oauth/access_token?grant_type=client_credentials&client_id={ClientID}&client_secret={ClientSecret}");

            if (result == null)
                return new AccessToken();

            return result;
        }

        public T GetAsync<T>(string endpoint)
        {
            var response = _httpClient.GetAsync($"{endpoint}").Result;

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return default(T);
        }

        public T GetAsync<T>(string endpoint, string args = null)
        {
            var response = _httpClient.GetAsync($"{endpoint}?access_token={AccessToken.Access_Token}" + (!string.IsNullOrEmpty(args) ? $"{args}" : "") ).Result;

            if (response.IsSuccessStatusCode) {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return default(T);
        }

        public Task PostAsync(string endpoint, object data, string args = null)
        {
            var payload = GetPayload(data);
            return _httpClient.PostAsync($"{endpoint}?access_token={AccessToken.Access_Token}&{args}", payload);
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
