using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Sitecore.Analytics;
using Sitecore.Analytics.Data.Items;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FX.Core.Utils
{
    public class Util
    {
        public static object DefaultRenderingCacheParameter = new { Cacheable = true, Cache_VaryByData = true };



        public static string SanitizeSitecorePath(string query)
        {
            string pattern = "/([\\w\\s\\-_]+)";
            Regex regex = new Regex(pattern);
            if (regex.Match(query) == null)
                return "";
            return regex.Replace(query, EscapeString);
        }

        // returns the escape string
        private static string EscapeString(Match match)
        {
            return string.Format("/#{0}#", match.Groups[1].Value);
        }

        public static bool HasValidLink(Link link, bool checkLinkText = true)
        {
            bool result = link != null && !string.IsNullOrEmpty(link.Url);
            if (result && checkLinkText)
            {
                result = !string.IsNullOrWhiteSpace(link.Text);
            }
            return result;
        }
        public static bool HasValidImage(Image image)
        {
            return (image != null && !string.IsNullOrEmpty(image.Src));
        }
        public static bool AreEquals(Guid guid, string id)
        {
            if (guid == Guid.Empty)
            {
                return false;
            }

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            return string.Equals(guid.ToString("B"), id, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasValidCarouselItem(Image image, Image mobileImage, Link link)
        {
            return (link != null && !string.IsNullOrEmpty(link.Url)) &&
                (image != null && !string.IsNullOrEmpty(image.Src)) &&
                (mobileImage != null && !string.IsNullOrEmpty(mobileImage.Src));
        }

        public static bool HasValidFeaturedItem(string title, Link link)
        {
            return !string.IsNullOrEmpty(title) &&
                (link != null && !string.IsNullOrEmpty(link.Url));
        }

        public static IEnumerable<List<T>> InSetsOf<T>(IEnumerable<T> source, int max) where T : class
        {
            var list = new List<T>(max);
            foreach (var item in source)
            {
                list.Add(item);
                if (list.Count == max)
                {
                    yield return list;
                    list = new List<T>(max);
                }
            }
            if (list.Any())
            {
                yield return list;
            }
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
                );
        }

        public static Item GetBannerItem()
        {
            var item = Sitecore.Context.Item;

            while (string.IsNullOrEmpty(item["BannerImage"]))
            {
                if (item.Parent != null)
                    item = item.Parent;
                else
                    return Sitecore.Context.Item;
            }

            return item;
        }

        public static string GetNormalizedId(Guid id)
        {
            return id.ToString("N").ToLower();
        }

        public static bool IsNewArticle(int daysLimit, DateTime articleDate, DateTime currentDate)
        {
            return daysLimit < 1 ? false : articleDate.AddDays(daysLimit) > currentDate;
        }

        public static bool HasEnquiryFormLink(FX.Core.Models.Base.ISitecoreItem form, string linkText)
        {
            return form != null && !string.IsNullOrEmpty(linkText);
        }

        public static string BuildQueryString(NameValueCollection collection)
        {
            string querystring = string.Empty;
            bool first = true;
            foreach (string key in collection.AllKeys)
            {
                if (first)
                {
                    querystring = string.Format("?{0}={1}", key, System.Web.HttpUtility.UrlEncode(collection[key]));
                    first = false;
                    continue;
                }
                querystring += string.Format("&{0}={1}", key, System.Web.HttpUtility.UrlEncode(collection[key]));
            }

            return querystring;
        }

        public static bool isDateExpired(DateTime compareFrom, DateTime compareTo)
        {
            if ((compareTo - compareFrom).TotalDays < 0)
            {
                return true;
            }
            return false;
        }

        public static string GetDuration(System.DateTime start, System.DateTime end)
        {
            List<string> durationsLabel = new List<string>();

            if (start != null)
            {
                if (start.Year > 1900)
                {
                    durationsLabel.Add(start.ToString("dd MMM yyyy"));
                }
            }

            if (end != null)
            {
                if (end.Year > 1900)
                {
                    durationsLabel.Add(end.ToString("dd MMM yyyy"));
                }
            }

            return String.Join(" - ", durationsLabel);
        }

        public static string MakeDayEvent(string summary, string location, DateTime startDate, DateTime endDate, string description, string country)
        {
            var icalStringbuilder = new StringBuilder();

            icalStringbuilder.AppendLine("BEGIN:VCALENDAR");
            icalStringbuilder.AppendLine("VERSION:2.0");
            if (!string.IsNullOrWhiteSpace(country))
            {
                icalStringbuilder.AppendLine("PRODID:-//Fuji Xerox " + country + "//NONSGML v1.0//EN");
            }
            else
            {
                icalStringbuilder.AppendLine("PRODID:-//Fuji Xerox//NONSGML v1.0//EN");
            }
            icalStringbuilder.AppendLine("BEGIN:VEVENT");
            // format the 2 timings to use 4 characters timestamps
            var timefrags = description.Split('-').Select(s => s.Replace(":", "").Trim().PadRight(4, '0'));
            var timestampFormat = "{0}T{1}00";
            string startDay;
            string endDay;
            if (timefrags.Count() < 2)
            {
                //if there is no end time we just default end to be 2359
                startDay = string.Format(timestampFormat, startDate.ToString("yyyyMMdd"), timefrags.FirstOrDefault());
                endDay = string.Format(timestampFormat, endDate.ToString("yyyyMMdd"), "2359");
            }
            else
            {

                startDay = string.Format(timestampFormat, startDate.ToString("yyyyMMdd"), timefrags.FirstOrDefault());
                endDay = string.Format(timestampFormat, endDate.ToString("yyyyMMdd"), timefrags.LastOrDefault());
            }
            /*
            string startDay = "VALUE=DATE:" + startDate.ToString("yyyyMMdd\\THHmmss");
            string endDay = "";
            if (endDate.ToString("yyyyMMdd\\THHmmss") != endDate.ToString("yyyyMMdd\\T000000"))
                endDay = "VALUE=DATE:" + endDate.ToString("yyyyMMdd\\THHmmss");
            else
                endDay = "VALUE=DATE:" + endDate.ToString("yyyyMMdd\\T235959");
            */
            icalStringbuilder.AppendLine("DTSTART:" + startDay);
            icalStringbuilder.AppendLine("DTEND:" + endDay);
            icalStringbuilder.AppendLine("SUMMARY:" + summary);
            icalStringbuilder.AppendLine("LOCATION:" + location.Replace("<br />", " "));
            //if (!string.IsNullOrEmpty(description))
            //{
            //    icalStringbuilder.AppendLine("DESCRIPTION:" + description.Replace("\r\n", "\\n"));
            //}
            icalStringbuilder.AppendLine("END:VEVENT");
            icalStringbuilder.AppendLine("END:VCALENDAR");

            return icalStringbuilder.ToString();
        }




        public static void TriggerTracker(string goalId)
        {
            if (!Tracker.IsActive || Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            if (Tracker.Current == null)
            {
                throw new Exception("Cannot activate tracker!");
            }

            Item goalItem = Sitecore.Context.Database.GetItem(goalId);

            if (goalItem == null)
            {
                throw new Exception("Goal NULL!");
            }

            var page = Tracker.Current.Session.Interaction.PreviousPage;

            if (page == null)
            {
                throw new Exception("Page is NULL!");
            }

            var registerGoal = new PageEventItem(goalItem);
            var eventData = page.Register(registerGoal);

            eventData.Data = goalItem["Description"];
            eventData.ItemId = goalItem.ID.Guid;
            eventData.DataKey = goalItem.Paths.Path;
            Tracker.Current.Interaction.AcceptModifications();

            Tracker.Current.CurrentPage.Cancel();
        }
        public static string OpcoType()
        {
            string opco = string.Empty;

            try
            {
                var item = Sitecore.Context.Database?.GetItem(Sitecore.Data.ID.Parse(FX.Core.Keys.Items.ProxyDictionaryOpcos));
                var datafield = item != null ? item[FX.Core.Keys.Items.OpcosDictionaryListField] : null;
                var nameValueCollection = datafield != null ? Sitecore.Web.WebUtil.ParseUrlParameters(datafield) : null;
                var startItem = Sitecore.Context.Site?.StartItem?.Split('/')?.Last();
                string[] startItemArray = !string.IsNullOrEmpty(startItem) ? nameValueCollection?.GetValues(startItem) : null;
                opco = startItemArray != null && startItemArray.Length > 0 ? startItemArray.FirstOrDefault() : string.Empty;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in FX.Core.Utils.Util -> OpcoType() :: {ex.ToString()}", ex);
            }

            return opco;
        }
        public static string OpcoLang()
        {
            string opcolang = string.Empty;

            try
            {
                var itemLang = Sitecore.Context.Database?.GetItem(Sitecore.Data.ID.Parse(FX.Core.Keys.Items.ProxyOpcosLangList));
                var datafieldlang = itemLang != null ? itemLang[FX.Core.Keys.Items.OpcosDictionaryListField] : null;
                var nameValueCollectionLang = datafieldlang != null ? Sitecore.Web.WebUtil.ParseUrlParameters(datafieldlang) : null;
                var startItem = Sitecore.Context.Site.StartItem?.Split('/')?.Last();
                string[] startItemArray = !string.IsNullOrEmpty(startItem) ? nameValueCollectionLang.GetValues(startItem) : null;
                opcolang = startItemArray != null && startItemArray.Length > 0 ? startItemArray.FirstOrDefault() : string.Empty;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error occurred in FX.Core.Utils.Util -> OpcoLang() :: {ex.ToString()}", ex);
            }

            return opcolang;
        }
    }
}
