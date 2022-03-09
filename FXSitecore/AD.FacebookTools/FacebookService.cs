using AD.FacebookTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.FacebookTools
{
    public class FacebookService
    {
        public string GetLatestPost(string clientID, string clientSecret, string profile_id)
        {
            var facebookClient = new FacebookClient(clientID, clientSecret);

            var posts = facebookClient.GetAsync<Posts>($"/v2.12/{profile_id}/posts", "&limit=1");

            if (posts == null || posts.data == null || !posts.data.Any())
                return string.Empty;


            return posts.data.FirstOrDefault().Message;
        }


    }
}
