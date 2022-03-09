using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using FX.Core.Utils;
using System.Xml;
using System.Text.RegularExpressions;
using Sitecore.Links;
using Sitecore.Sites;
using System.Linq;
using Sitecore.Data;

namespace FX.Core.Search.ComputedFields
{
    public class ComputeItemUrl : AbstractComputedIndexField
    {
        public ComputeItemUrl(XmlNode configNode) : base(configNode)
        {
            HomeID = Sitecore.Xml.XmlUtil.GetAttribute("homeID", configNode);
        }

        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item != null)
            {
                var homeItem = GetHome(item);

                if (homeItem != null)
                {
                    var site = GetSite(homeItem);

                    if (site != null)
                    {
                        string targetHostName = site.TargetHostName;
                        UrlOptions defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                        defaultUrlOptions.Site = new SiteContext(site);
                        defaultUrlOptions.LanguageEmbedding = LanguageEmbedding.Always;
                        defaultUrlOptions.Language = item.Language;
                        string s =  LinkManager.GetItemUrl(item, defaultUrlOptions).Replace(":", "");

                        s = s.Replace($"//{targetHostName}", "");

                        return s;
                    }
                    return item.Paths.Path.Replace(homeItem.Paths.Path, "").Replace(' ', '-');
                }
                else
                {
                    UrlOptions defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                    defaultUrlOptions.Site = SiteContextFactory.GetSiteContext("website");
                    defaultUrlOptions.LanguageEmbedding = LanguageEmbedding.Always;
                    defaultUrlOptions.Language = item.Language;
                    string s = LinkManager.GetItemUrl(item, defaultUrlOptions);
                    return s;
                }
            }

            return null;
        }

        private Item GetHome(Item item)
        {
            if (item == null || !item.Paths.Path.ToLower().StartsWith("/sitecore/content"))
                return null;
            var homeItem = item.Axes.GetAncestors().Where(i => i.TemplateID == ID.Parse(HomeID)).FirstOrDefault();
            //var homeItem = item.Database.SelectSingleItem(string.Format("{0}/ancestor-or-self::*[@@templateid='{1}']", EscapeQuery(item.Paths.Path), HomeID));

            return homeItem;
        }

        public string HomeID { get; set; }

        public string EscapeQuery(string query)
        {
            string pattern = "/([\\w\\s\\-_]+)";
            Regex regex = new Regex(pattern);
            if (regex.Match(query) == null)
                return "";
            return regex.Replace(query, EscapeString);
        }

        private string EscapeString(Match match)
        {
            return string.Format("/#{0}#", match.Groups[1].Value);
        }

        public static Sitecore.Web.SiteInfo GetSite(Item item)
        {
            var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();

            return siteInfoList
                .Where(s => (s.RootPath + s.StartItem).StartsWith(item.Paths.FullPath, System.StringComparison.InvariantCultureIgnoreCase))
                //we do not want any other special site or what not.
                .Where(s => s.PhysicalFolder == "/")
                .FirstOrDefault();

        }
    }
}
