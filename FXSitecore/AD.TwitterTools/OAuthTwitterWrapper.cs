using AD.TwitterTools.Interfaces;
using AD.TwitterTools.JsonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.TwitterTools
{
    public class OAuthTwitterWrapper : IOAuthTwitterWrapper
    {
        public IAuthenticateSettings AuthenticateSettings { get; set; }

        public OAuthTwitterWrapper(string consumerKey,string consumerSecret,string authUrl)
        {
            AuthenticateSettings = new AuthenticateSettings { OAuthConsumerKey = consumerKey, OAuthConsumerSecret = consumerSecret, OAuthUrl = authUrl };
        }

        /// <summary>
        /// This allows the authentications settings to be passed in
        /// </summary>
        /// <param name="authenticateSettings"></param>
        public OAuthTwitterWrapper(IAuthenticateSettings authenticateSettings)
        {
            AuthenticateSettings = authenticateSettings;
        }

        //public string GetMyTimeline(TimeLineSettings TimeLineSettings)
        //{
        //    var timeLineJson = string.Empty;
        //    IAuthenticate authenticate = new Authenticate();
        //    AuthResponse twitAuthResponse = authenticate.AuthenticateMe(AuthenticateSettings);

        //    // Do the timeline
        //    var utility = new Utility();
        //    timeLineJson = utility.RequstJson(TimeLineSettings.TimelineUrl, twitAuthResponse.TokenType, twitAuthResponse.AccessToken);

        //    return timeLineJson;
        //}

        public string GetSearch(string searchFormat, string searchQuery)
        { 
            return GetSearch(new SearchSettings(){ SearchFormat= searchFormat, SearchQuery = searchQuery});
        }

        public string GetSearch(ISearchSettings SearchSettings)
        {
            var searchJson = string.Empty;
            IAuthenticate authenticate = new Authenticate();
            AuthResponse twitAuthResponse = authenticate.AuthenticateMe(AuthenticateSettings);

            var utility = new Utility();
            searchJson = utility.RequstJson(SearchSettings.SearchUrl, twitAuthResponse.TokenType, twitAuthResponse.AccessToken);

            return searchJson;
        }
    }
}
