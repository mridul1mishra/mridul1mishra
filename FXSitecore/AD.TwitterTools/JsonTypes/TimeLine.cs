using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace AD.TwitterTools.JsonTypes
{

    public class TimeLine
    {

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("id_str")]
		public string IdStr { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("truncated")]
		public bool Truncated { get; set; }

		[JsonProperty("in_reply_to_status_id")]
		public string InReplyToStatusId { get; set; }

		[JsonProperty("in_reply_to_status_id_str")]
		public string InReplyToStatusIdStr { get; set; }

		[JsonProperty("in_reply_to_user_id")]
		public string InReplyToUserId { get; set; }

		[JsonProperty("in_reply_to_user_id_str")]
		public string InReplyToUserIdStr { get; set; }

		[JsonProperty("in_reply_to_screen_name")]
		public string InReplyToScreenName { get; set; }

		[JsonProperty("user")]
		public User User { get; set; }

		[JsonProperty("geo")]
		public string Geo { get; set; }

		[JsonProperty("coordinates")]
		public string Coordinates { get; set; }

		[JsonProperty("place")]
		public string Place { get; set; }

		[JsonProperty("contributors")]
		public string Contributors { get; set; }

		[JsonProperty("retweet_count")]
		public int RetweetCount { get; set; }

		[JsonProperty("favorite_count")]
		public int FavoriteCount { get; set; }

		[JsonProperty("entities")]
		public Entities Entities { get; set; }

		[JsonProperty("favorited")]
		public bool Favorited { get; set; }

		[JsonProperty("retweeted")]
		public bool Retweeted { get; set; }

		[JsonProperty("lang")]
		public string Lang { get; set; }

        public string ProcessText()
        {
            string text = this.Text;

            //process urls
            foreach(var urlEntity in this.Entities.Urls)
            {
                text = text.Replace(urlEntity.UrlValue, $"<a href=\"{urlEntity.UrlValue}\" target=\"_blank\">{urlEntity.UrlValue}</a>");
            }
            //process mentions
            foreach(var userMentionEntity in this.Entities.UserMentions)
            {
                text = text.Replace(userMentionEntity.ScreenName, $"<a href=\"https://www.twitter.com/{userMentionEntity.ScreenName}\" target=\"_blank\">{userMentionEntity.ScreenName}</a>");
            }
            
            return text;
        }
    }

}
