using AD.FacebookTools;
using AD.TwitterTools;
using AD.TwitterTools.JsonTypes;
using FX.Core.Utils;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Tasks
{
    public class SocialMediaGetLatestPosts
    {
        public void Run()
        {
            // find all
            var datasources = GetSocialFeedItems();
            var now = DateTime.Now.ToLocalTime();
            // loop through all datasources found
            foreach (var datasource in datasources)
            {
                DateField lastRunField = (DateField)datasource.Fields["LastRun"];
                if (lastRunField.DateTime.ToLocalTime().AddHours(1) <= now)
                {
                    string facebookMessage = GetFacebook(datasource);
                    var twitterTimeLine = GetTwitter(datasource);
                    string liMessage = GetLinkedIn(datasource);
                    var ytResult = GetYoutube(datasource);
                    using (new SecurityDisabler())
                    {
                        datasource.Editing.BeginEdit();
                        {
                            datasource["FBText"] = !string.IsNullOrEmpty(facebookMessage) ? facebookMessage : datasource["FBText"];
                            if (twitterTimeLine != null)
                            {
                                string twitterMessage = twitterTimeLine.ProcessText();
                                datasource["TWText"] = !string.IsNullOrEmpty(twitterMessage) ? twitterMessage : datasource["TWText"];
                                datasource["TweetID"] = twitterTimeLine.Id;
                            }

                            datasource["LIText"] = !string.IsNullOrEmpty(liMessage) ? liMessage : datasource["LIText"];
                            if (ytResult != null)
                            {
                                datasource["YTText"] = !string.IsNullOrEmpty(ytResult.Text) ? ytResult.Text : datasource["YTText"];
                                datasource["YTVideoID"] = !string.IsNullOrEmpty(ytResult.VideoID) ? ytResult.VideoID : datasource["YTVideoID"];
                            }

                            datasource["LastRun"] = now.ToUniversalTime().ToString("yyyyMMddThhmmssZ");
                        }
                        datasource.Editing.EndEdit();
                        PublishItem(datasource);
                    }
                }
            }
        }

        private void PublishItem(Item item)
        {
            Sitecore.Publishing.PublishOptions publishOptions =
                new Sitecore.Publishing.PublishOptions(item.Database,
                                           Database.GetDatabase("web"),
                                           Sitecore.Publishing.PublishMode.Incremental,
                                           item.Language,
                                           DateTime.Now.ToUniversalTime());  // Create a publisher with the publishoptions
            Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

            // Choose where to publish from
            publisher.Options.RootItem = item;

            // Publish children as well?
            publisher.Options.Deep = true;

            // Do the publish!
            publisher.Publish();
        }

        private string GetFacebook(Item datasource)
        {
            if (datasource["FBEnabled"] == "1")
            {
                try
                {
                    var fbService = new FacebookService();
                    var search = fbService.GetLatestPost(datasource["FBClientID"], datasource["FBClientSecret"], datasource["FBProfileID"]);
                    Sitecore.Diagnostics.Log.Info($"Finished getting feed from facebook\n", this);
                    return search;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error($"\nCouldnt get feed from facebook.\n Datasource ID: {datasource.ID.ToString()}\n {ex.Message}\n {ex.StackTrace}", this);
                }
            }
            return null;
        }

        private TimeLine GetTwitter(Item datasource)
        {
            if (datasource["TWEnabled"] == "1")
            {
                try
                {
                    string consumerKey = datasource["TWConsumerKey"];
                    string consumerSecret = datasource["TWConsumerSecret"];
                    string authUrl = "https://api.twitter.com/oauth2/token";
                    string screenName = datasource["TWScreenName"];

                    OAuthTwitterWrapper twitterWrapper = new OAuthTwitterWrapper(consumerKey, consumerSecret, authUrl);

                    string format = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&count=1";
                    var json = twitterWrapper.GetSearch(format, screenName);
                    var result = JsonConvert.DeserializeObject<TimeLine[]>(json);

                    var tweet = result.FirstOrDefault();
                    Sitecore.Diagnostics.Log.Info($"Finished getting feed from twitter\n", this);
                    return tweet;
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error($"\nCouldnt get feed from Twitter.\n Datasource ID: {datasource.ID.ToString()}\n {ex.Message}\n {ex.StackTrace}", this);
                }
            }
            return null;
        }

        private string GetLinkedIn(Item datasource)
        {
            if (datasource["LIEnabled"] == "1")
            {
                try
                {
                    string companyID = datasource["LICompanyID"];
                    string clientID = datasource["LIClientID"];
                    string clientSecret = datasource["LIClientSecret"];
                    string redirectUrl = datasource["LIRedirectUrl"];

                    return AD.LinkedIn.Utility.GetLatestUpdate(companyID, clientID, clientSecret, redirectUrl);

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error($"\nCouldnt get feed from Linked in.\n Datasource ID: {datasource.ID.ToString()}\n {ex.Message}\n {ex.StackTrace}", this);
                }
            }
            return string.Empty;
        }

        private YoutubeResult GetYoutube(Item datasource)
        {
            if (datasource["YTEnabled"] == "1")
            {
                var search = AD.Youtube.Utility.GetLatestVideo(datasource["YTApiKey"], datasource["YTChannelID"]);
                var result = new YoutubeResult();

                var ytItem = search?.items?.FirstOrDefault();
                if (ytItem != null)
                {
                    result.Title = ytItem.snippet.title;
                    result.VideoID = ytItem.id.videoId;
                    result.Image = ytItem.snippet.thumbnails.high.url;

                    return result;
                }
                else
                {
                    return null;
                }

            }
            return null;
        }
        private List<Item> GetSocialFeedItems()
        {
            List<Item> items = new List<Item>();
            var db = Sitecore.Data.Database.GetDatabase("master");
            items.AddRange(db.SelectItems($"fast:/sitecore/content//*[@@templateid='{Templates.SocialFeed.Id}']"));

            return items;
        }
    }

    public class YoutubeResult
    {
        public string Title;
        public string VideoID;
        public string Text
        {
            get
            {
                return $"<p><a href=\"https://youtube.com/watch?v={VideoID}\" target=\"_blank\"><img src=\"{Image}\"/></a></p><p>{Title}</p>";
            }
        }
        public string Image;
    }
}
