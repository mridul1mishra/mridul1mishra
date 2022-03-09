using FX.Core.GlassMapper;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
using FX.Core.Search.Models;
using FX.Core.Search.Models.Taxonomy;
using Glass.Mapper.Sc;
using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.Core.Utils
{
	public class NewsroomService
	{
		public ISitecoreContext Context { get; private set; }

		public NewsroomService() : this(null) { }

		public NewsroomService(ISitecoreContext context)
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

		protected string GetItemId(ITaxonomyItem item)
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

		public IEnumerable<FXResultItem> SearchNews(string category, int year, int page, int pageSize, string orderBy, out int hitsCount)
		{
			hitsCount = 0;
			if (string.IsNullOrEmpty(category))
			{
				return new FXResultItem[0];
			}

            
            var whereClause = PredicateBuilder.True<FXResultItem>();

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
			whereClause = whereClause.And(x => x.Paths.Contains(TryParseId(category)));
			if (year > 0)
			{
				DateTime startYear = new DateTime(year, 1, 1);
				DateTime endYear = new DateTime(year, 12, 31, 23, 59, 59);
				whereClause = whereClause.And(x => x.ArticleDate >= startYear && x.ArticleDate <= endYear);
			}

			var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
			searchParam.AddTemplateRestriction(new ID(Templates.StandardPage.Id));
			searchParam.Page = page;
			searchParam.PageSize = pageSize;
			searchParam.WhereClause = whereClause;
			searchParam.OrderBy = results => results.OrderByDescending(x => x.ArticleDate);
			if (orderBy != null)
			{
				// do nothing
			}

			var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
			return searchResults.ToArray();
		}

		public JObject GetNewsListing(string category, int year, int page, int pageSize, string orderBy)
		{
			try
			{
				pageSize = Math.Abs(pageSize);
				int hitsCount;
				var searchResults = SearchNews(category, year, page, pageSize, orderBy, out hitsCount);
                var opco = FX.Core.Utils.Util.OpcoType();

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
					news = from item in searchResults
						   select new
						   {
							   title = item.TeaserTitle,
							   date = item.LocalisedArticleDate.DayMonthYearString,
							   year = item.LocalisedArticleDate.DateTime.Year,
							   url = "/"+opco+item.Url,
						   },
					totalPage = totalPage
				});
				return result;
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("Failed to get news listing under {0} within {1}", category, year), ex);

				var array = new List<JObject>();
				return JObject.FromObject(new
				{
					news = array,
					totalPage = 0
				});
			}
		}

		public IEnumerable<FXResultItem> GetLatestNews(Guid[] categoryIds, int pageSize)
		{
			if (categoryIds == null || categoryIds.Count() == 0)
			{
				return new FXResultItem[0];
			}

            var homePageId = new ID(HomePage.Id);

			var whereClause = PredicateBuilder.True<FXResultItem>();
           // whereClause = whereClause.And(x => x.Paths.Contains(homePageId));
          //  whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
			foreach (var categoryId in categoryIds)
			{
                whereClause = whereClause.Or(x => x.Paths.Contains(ID.Parse(categoryId)));
			}

            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));
            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);

			var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
			searchParam.AddTemplateRestriction(new ID(Templates.StandardPage.Id));
			searchParam.Page = 1;
			searchParam.PageSize = pageSize;
			searchParam.WhereClause = whereClause;
			searchParam.OrderBy = results => results.OrderByDescending(x => x.ArticleDate);

			int hitsCount = 0;
			var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
			return searchResults.ToArray();
		}
	}
}
