// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AD.TwitterTools.JsonTypes
{

    public class User
    {

		[JsonProperty("id")]
		public Int64 Id { get; set; }

		[JsonProperty("id_str")]
		public string IdStr { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("screen_name")]
		public string ScreenName { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("entities")]
		public UserEntities Entities { get; set; }

		[JsonProperty("protected")]
		public bool Protected { get; set; }

		[JsonProperty("followers_count")]
		public int FollowersCount { get; set; }

		[JsonProperty("friends_count")]
		public int FriendsCount { get; set; }

		[JsonProperty("listed_count")]
		public int ListedCount { get; set; }

		[JsonProperty("created_at")]
		public string CreatedAt { get; set; }

		[JsonProperty("favourites_count")]
		public int FavouritesCount { get; set; }

		[JsonProperty("utc_offset")]
		public string UtcOffset { get; set; }

		[JsonProperty("time_zone")]
		public string TimeZone { get; set; }

		[JsonProperty("geo_enabled")]
		public bool GeoEnabled { get; set; }

		[JsonProperty("verified")]
		public bool Verified { get; set; }

		[JsonProperty("statuses_count")]
		public int StatusesCount { get; set; }

		[JsonProperty("lang")]
		public string Lang { get; set; }

		[JsonProperty("contributors_enabled")]
		public bool ContributorsEnabled { get; set; }

		[JsonProperty("is_translator")]
		public bool IsTranslator { get; set; }

		[JsonProperty("profile_background_color")]
		public string ProfileBackgroundColor { get; set; }

		[JsonProperty("profile_background_image_url")]
		public string ProfileBackgroundImageUrl { get; set; }

		[JsonProperty("profile_background_image_url_https")]
		public string ProfileBackgroundImageUrlHttps { get; set; }

		[JsonProperty("profile_background_tile")]
		public bool ProfileBackgroundTile { get; set; }

		[JsonProperty("profile_image_url")]
		public string ProfileImageUrl { get; set; }

		[JsonProperty("profile_image_url_https")]
		public string ProfileImageUrlHttps { get; set; }

		[JsonProperty("profile_link_color")]
		public string ProfileLinkColor { get; set; }

		[JsonProperty("profile_sidebar_border_color")]
		public string ProfileSidebarBorderColor { get; set; }

		[JsonProperty("profile_sidebar_fill_color")]
		public string ProfileSidebarFillColor { get; set; }

		[JsonProperty("profile_text_color")]
		public string ProfileTextColor { get; set; }

		[JsonProperty("profile_use_background_image")]
		public bool ProfileUseBackgroundImage { get; set; }

		[JsonProperty("default_profile")]
		public bool DefaultProfile { get; set; }

		[JsonProperty("default_profile_image")]
		public bool DefaultProfileImage { get; set; }

		[JsonProperty("following")]
		public string Following { get; set; }

		[JsonProperty("follow_request_sent")]
		public string FollowRequestSent { get; set; }

		[JsonProperty("notifications")]
		public string Notifications { get; set; }
    }

}
