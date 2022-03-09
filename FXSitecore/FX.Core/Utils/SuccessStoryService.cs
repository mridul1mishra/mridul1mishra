using FX.Core.GlassMapper;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using FX.Core.Models.Settings.SuccessStories;
using FX.Core.Search.Models;
using FX.Core.Search;
using Glass.Mapper.Sc;
using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using System.Collections;

namespace FX.Core.Utils
{
    public class SuccessStoryService
    {
        public ISitecoreContext Context { get; private set; }

        public SuccessStoryService() : this(null) { }

        public SuccessStoryService(ISitecoreContext context)
        {
            if (context == null)
            {
                context = new SitecoreContext();
            }
            this.Context = context;
        }

        private IHomePage _homePage;

        public IHomePage HomePage
        {
            get
            {
                if (this._homePage == null)
                {
                    this._homePage = Context.GetHomeItem<IHomePage>();
                }
                return this._homePage;
            }
        }

        protected string GetItemId(ISuccessStoryDetailPage item)
        {
            return Util.GetNormalizedId(item.Id);
        }

        protected ID TryParseId(string value)
        {
            ID result = ID.Null;
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            ShortID sId;
            if (ShortID.TryParse(value, out sId))
            {
                result = sId.ToID();
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid id: {0}", value), "value");
            }
            return result;
        }

        public IEnumerable<FXResultItem> SearchSuccessStories(string country, string solution, string product, string industry, string year, out int hitsCount, int page = 0, int pageSize = 0)
        {
            hitsCount = 0;
            var homePageId = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));


            if (!string.IsNullOrWhiteSpace(country))
            {
                whereClause = whereClause.And(x => x.SuccessStoriesCountries.Contains(TryParseId(country)));
            }

            if (!string.IsNullOrWhiteSpace(solution))
            {
                whereClause = whereClause.And(x => x.SuccessStoriesSolutions.Contains(TryParseId(solution)));
            }

            if (!string.IsNullOrWhiteSpace(product))
            {
                whereClause = whereClause.And(x => x.SuccessStoriesProducts.Contains(TryParseId(product)));
            }

            if (!string.IsNullOrWhiteSpace(industry))
            {
                whereClause = whereClause.And(x => x.SuccessStoriesIndustries.Contains(TryParseId(industry)));
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                whereClause = whereClause.And(x => x.SuccessStoriesYear.Contains(TryParseId(year)));
            }
            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.SuccessStoryDetailPage.Id));
            searchParam.WhereClause = whereClause;
            searchParam.OrderBy = results => results.OrderByDescending(x => x.ArticleDate);

            searchParam.PageSize = pageSize;
            searchParam.Page = page;

            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);

            return searchResults;
        }

        private string GetIndustry(IEnumerable<ID> industries)
        {
            List<string> industriesLabel = new List<string>();

            foreach (var industry in industries)
            {
                var filterItem = this.Context.GetItem<ISuccessStoriesFilterItem>(industry.Guid);
                //var filterItem = Context.Cast<ISuccessStoriesFilterItem>(filterItemRaw);
                if (filterItem != null)
                    industriesLabel.Add(filterItem.SuccessStoryFilterItemName);
            }

            return String.Join(", ", industriesLabel);
        }

        private string GetLocation(IEnumerable<ID> locations)
        {
            List<string> locationsLabel = new List<string>();

            foreach (var location in locations)
            {
                var filterItem = this.Context.GetItem<ISuccessStoriesFilterItem>(location.Guid);
                //var filterItem = Context.Cast<ISuccessStoriesFilterItem>(filterItemRaw);
                if (filterItem != null)
                    locationsLabel.Add(filterItem.SuccessStoryFilterItemName);
            }

            return String.Join(", ", locationsLabel);
        }

        public JObject GetSuccessStoriesListing(string country, string solution, string product, string industry, string year, int page, int pageSize)
        {
            //int getCount = 9;
            try
            {
                pageSize = Math.Abs(pageSize);
                int hitsCount = 0;
                var searchResults = SearchSuccessStories(country, solution, product, industry, year, out hitsCount, page, pageSize);

                int totalPage = 0;
                if (hitsCount > 0)
                {
                    totalPage = 1;
                    if (pageSize > 0)
                    {
                        totalPage += ((hitsCount - 1) / pageSize);
                    }
                }
                var result = JObject.FromObject(new
                {
                    count = hitsCount,
                    totalPage = totalPage,
                    results = from item in searchResults
                              select new
                              {
                                  title = item.TeaserTitle,
                                  thumbnail = item.TeaserImage == null ? string.Empty : $"/{FX.Core.Utils.Util.OpcoType()}{item.TeaserImage.Url}",
                                  link = $"/{FX.Core.Utils.Util.OpcoType()}{item.Url}",
                                  text = item.TeaserDescription == null ? string.Empty : item.TeaserDescription,
                                  industry = item.SuccessStoriesIndustries == null ? string.Empty : GetIndustry(item.SuccessStoriesIndustries),
                                  location = item.SuccessStoriesCountries == null ? string.Empty : GetLocation(item.SuccessStoriesCountries),
                                  buttontext = item.ButtonText == null ? string.Empty : item.ButtonText,
                              }

                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get success stories listing."), ex);
                Log.Error(ex.StackTrace, ex);
                var array = new List<JObject>();
                return JObject.FromObject(new
                {
                    count = 0,
                    endIndex = 0,
                    results = array
                });
            }
        }
    }
}
