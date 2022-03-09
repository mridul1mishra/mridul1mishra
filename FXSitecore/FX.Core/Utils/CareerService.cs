using FX.Core.Models.DTO;
using FX.Core.Models.Page;
using FX.Core.Models.Settings.Careers;
using FX.Core.Search.Models;
using Glass.Mapper.Sc;
using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FX.Core.Utils
{
    public class CareerService
	{
		public ISitecoreContext Context { get; private set; }

		public CareerService() : this(null) { }

        public CareerService(ISitecoreContext context)
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

		protected string GetItemId(ICareerDetailPage item)
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

        public IEnumerable<FXResultItem> SearchCareers(CareerSearch args, out int hitsCount)
        {
            hitsCount = 0;
            var homePageId = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));

            whereClause = args.Arguments.Aggregate(whereClause,
                (p, a) =>
                {
                    if (a != null && a.Values != null && a.Values.Any())
                    {
                        var clause = PredicateBuilder.True<FXResultItem>();
                        clause = a.Values.Aggregate(clause, (pr, v) => pr.Or(f => f[a.Key].Contains(v)));
                        p = p.And(clause);
                    }
                    return p;
                });
            if (args.DateItemArgument != null && args.DateItemArgument.Values != null && args.DateItemArgument.Values.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                clause = args.DateItemArgument.Values.Aggregate(clause, (p, v) =>
                {
                    var careerFilterItem = Context.GetItem<ICareersDateFilterItem>(TryParseId(v).Guid);
                    //Need to use UTC datetime since sitecore is using UTC datetime
                    DateTime nowDate = DateTime.Now.ToUniversalTime().AddHours(8);
                    DateTime prevDate = DateTime.Now.AddDays(-careerFilterItem.NumberOfDays).ToUniversalTime().AddHours(8);
                    return p.Or(x => x.JobPostingDate >= prevDate && x.JobPostingDate <= nowDate);
                });
                whereClause = whereClause.And(clause);
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.CareerDetailPage.Id));
            searchParam.WhereClause = whereClause;
            searchParam.OrderBy = results => results.OrderByDescending(x => x.JobPostingDate);

            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return searchResults.ToArray();
        }

        /*
        public IEnumerable<FXResultItem> SearchCareers(string[] careerlocations, string[] careerspecialisations, string[] careerjobtypes, string[] careertitles, string[] careerpostingdates, out int hitsCount)
        {
            hitsCount = 0;
            var homePageId = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));

            if (careerlocations != null && careerlocations.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                foreach (var careerlocation in careerlocations)
                {
                    clause = clause.Or(x => x.CareerLocation.Contains(TryParseId(careerlocation)));
                }
                whereClause = whereClause.And(clause);
            }

            if (careerspecialisations != null && careerspecialisations.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                foreach (var careerspecialisation in careerspecialisations)
                {
                    clause = clause.Or(x => x.CareerSpecialisation.Contains(TryParseId(careerspecialisation)));
                }
                whereClause = whereClause.And(clause);
            }

            if (careerjobtypes != null && careerjobtypes.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                foreach (var careerjobtype in careerjobtypes)
                {
                    clause = clause.Or(x => x.CareerJobType.Contains(TryParseId(careerjobtype)));
                }
                whereClause = whereClause.And(clause);
            }

            if (careertitles != null && careertitles.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                foreach (var careertitle in careertitles)
                {
                    clause = clause.Or(x => x.CareerTitle.Contains(TryParseId(careertitle)));
                }
                whereClause = whereClause.And(clause);
            }

            if (careerpostingdates != null && careerpostingdates.Any())
            {
                var clause = PredicateBuilder.True<FXResultItem>();
                foreach (var careerpostingdate in careerpostingdates)
                {
                    var careerFilterItem = Context.GetItem<ICareersDateFilterItem>(TryParseId(careerpostingdate).Guid);

                    //Need to use UTC datetime since sitecore is using UTC datetime
                    DateTime nowDate = DateTime.Now.ToUniversalTime().AddHours(8);
                    DateTime prevDate = DateTime.Now.AddDays(-careerFilterItem.NumberOfDays).ToUniversalTime().AddHours(8);

                    clause = clause.Or(x => x.JobPostingDate >= prevDate && x.JobPostingDate <= nowDate);
                }
                whereClause = whereClause.And(clause);
            }

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.CareerDetailPage.Id));
            searchParam.WhereClause = whereClause;
            searchParam.OrderBy = results => results.OrderByDescending(x => x.JobPostingDate);

            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            return searchResults.ToArray();
        }*/

        private string GetSpecialisations(IEnumerable<ID> specialisations)
        {
            List<string> specialisationsLabel = new List<string>();

            foreach (var specialisation in specialisations)
            {
                var filterItemRaw = this.Context.Database.GetItem(specialisation);
                var filterItem = Context.Cast<ICareersFilterItem>(filterItemRaw);
                specialisationsLabel.Add(filterItem.Name);
            }

            return String.Join(", ", specialisationsLabel);
        }

        private string GetLocation(IEnumerable<ID> locations)
        {
            List<string> locationsLabel = new List<string>();

            foreach (var location in locations)
            {
                var filterItemRaw = this.Context.Database.GetItem(location);
                var filterItem = Context.Cast<ICareersFilterItem>(filterItemRaw);
                locationsLabel.Add(filterItem.Name);
            }

            return String.Join(", ", locationsLabel);
        }

        private string GetDisplayTimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.Subtract(dateTime.ToUniversalTime());

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(7))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days/7 > 1 ?
                    String.Format("about {0} weeks ago", timeSpan.Days / 7) :
                    "about a week ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }

        public JObject GetCareerListing(CareerSearch searchArgs)
        {
            try
            {
                int hitsCount;
                var searchResults = SearchCareers(searchArgs, out hitsCount);
                string opco = Util.OpcoType();

                var result = JObject.FromObject(new
                {
                    results = from item in searchResults
                              select new
                              {
                                  title = item.TeaserTitle == null ? string.Empty : item.TeaserTitle,
                                  specialisation = item.Specialization == null ? string.Empty : item.Specialization,
                                  url = $"/{opco}{item.Url}",
                                  date = item.LocalisedJobPostingDate == null ? string.Empty : item.LocalisedJobPostingDate.DayMonthYearString,//.ToString(FX.Core.Constants.DateTimeFormat.FullDateTime),
                                  //timestamp = item.LocalisedJobPostingDate == null ? string.Empty : GetDisplayTimeAgo(item.LocalisedJobPostingDate.DateTime),
                                  timestamp = item.LocalisedJobPostingDate == null ? string.Empty : item.LocalisedJobPostingDate.DayMonthYearString,
                                  location = item.CareerLocation == null ? string.Empty : item.CareerLocation,
                                  careerId = string.Empty
                              }
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get careers listing."), ex);
                Log.Error(ex.StackTrace, ex);
                var array = new List<JObject>();
                return JObject.FromObject(new
                {
                    results = array
                });
            }
        }

        /*public JObject GetCareerListing(string[] careerlocations, string[] careerspecialisations, string[] careerjobtypes, string[] careertitles, string[] careerpostingdates)
        {
            try
            {
                int hitsCount;
                var searchResults = SearchCareers(careerlocations, careerspecialisations, careerjobtypes, careertitles, careerpostingdates, out hitsCount);

                var result = JObject.FromObject(new
                {
                    results = from item in searchResults
                              select new
                              {
                                  title = item.TeaserTitle == null ? string.Empty : item.TeaserTitle,
                                  specialisation = item.CareerSpecialisation == null ? string.Empty : GetSpecialisations(item.CareerSpecialisation),
                                  url = item.Url,
                                  date = item.JobPostingDate == null ? string.Empty : item.JobPostingDate.ToString(FX.Core.Constants.DateTimeFormat.FullDateTime),
                                  timestamp = item.JobPostingDate == null ? string.Empty : GetDisplayTimeAgo(item.JobPostingDate.ToUniversalTime()),
                                  location = item.CareerLocation == null ? string.Empty : GetSpecialisations(item.CareerLocation),
                                  careerId = string.Empty
                              }  
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get careers listing."), ex);
                Log.Error(ex.StackTrace, ex);
                var array = new List<JObject>();
                return JObject.FromObject(new
                {
                    results = array
                });
            }
        }*/
	}
}
