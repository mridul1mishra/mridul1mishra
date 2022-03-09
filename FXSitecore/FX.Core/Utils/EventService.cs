using FX.Core.GlassMapper;
using FX.Core.Models.DTO;
using FX.Core.Models.Page;
using FX.Core.Models.Settings;
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
using FX.Core.Models.Settings.Events;

namespace FX.Core.Utils
{
	public class EventService
	{
		public ISitecoreContext Context { get; private set; }

		public EventService() : this(null) { }

        public EventService(ISitecoreContext context)
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

		protected string GetItemId(IEventDetailPage item)
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

        public string MakeDayEvent(string eventId)
        {
            Database database = Sitecore.Context.Database;
            Item item = database.GetItem(TryParseId(eventId));

			if (item == null)
            {
                return "";
            }

            var service = new Glass.Mapper.Sc.SitecoreService(Sitecore.Context.Database);
            if (Util.AreEquals(item.TemplateID.Guid, Templates.EventDetailPage.Id))
            {
                var eventPage = service.Cast<FX.Core.Models.Page.IEventDetailPage>(item);
                return FX.Core.Utils.Util.MakeDayEvent(eventPage.MainTitle, eventPage.EventLocation, eventPage.EventStartDate, eventPage.EventEndDate, eventPage.StartEndTime, "");
            }
            else if (Util.AreEquals(item.TemplateID.Guid, Templates.EventRegistrationPage.Id))
            {
                var eventRegPage = service.Cast<FX.Core.Models.Page.IEventRegistrationPage>(item);
                return FX.Core.Utils.Util.MakeDayEvent(eventRegPage.MainTitle, eventRegPage.Location, eventRegPage.StartDate, eventRegPage.EndDate, eventRegPage.StartEndTime, "");
            }
            else
            {
                return "";
            }
        }

        public IEnumerable<FXResultItem> SearchEvents(string country, string industry, string year, string month, out int hitsCount, int page, int pageSize)
        {
            hitsCount = 0;
            var homePageId = new ID(HomePage.Id);
            var whereClause = PredicateBuilder.True<FXResultItem>();

            whereClause = whereClause.And(x => x.Language == Sitecore.Context.Language.Name);
            whereClause = whereClause.And(x => x.Paths.Contains(homePageId));

            if (!string.IsNullOrWhiteSpace(country))
            {
                whereClause = whereClause.And(x => x.EventLocations.Contains(TryParseId(country)));
            }

            if (!string.IsNullOrWhiteSpace(industry))
            {
                whereClause = whereClause.And(x => x.EventIndustries.Contains(TryParseId(industry)));
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                whereClause = whereClause.And(x => x.EventYear.Contains(TryParseId(year)));
            }
            else
            {
                //by default hide expired events when year is not selected
                whereClause = whereClause.And(i => i.EventEndDate >= DateTime.Today.ToUniversalTime());
            }

            if (!string.IsNullOrWhiteSpace(month))
            {
                whereClause = whereClause.And(x => x.EventMonth.Contains(TryParseId(month)));
            }
            

            var searchParam = new SearchParam<FXResultItem>(this.Context.Database);
            searchParam.AddTemplateRestriction(new ID(Templates.EventDetailPage.Id));
            searchParam.WhereClause = whereClause;
            searchParam.PageSize = pageSize;
            searchParam.Page = page;

            var searchResults = SearchHelper.Search<FXResultItem>(searchParam, out hitsCount);
            searchResults = searchResults.OrderByDescending(e => e.EventStartDate);

            return searchResults;
        }

        private string GetIndustry(IEnumerable<ID> industries)
        {
            List<string> industriesLabel = new List<string>();

            foreach(var industry in industries)
            {
                var filterItemRaw = this.Context.Database.GetItem(industry);
                var filterItem = Context.Cast<IEventsFilterItem>(filterItemRaw);
                industriesLabel.Add(filterItem.Name);
            }

            return String.Join(", ", industriesLabel);
        }

        private string GetDuration(System.DateTime start, System.DateTime end)
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

        public JObject GetEventsListing(string country, string industry, string year, string month, int page, int pageSize)
        {
            try
            {
                pageSize = Math.Abs(pageSize);
                int hitsCount = 0;
                var searchResults = SearchEvents(country, industry, year, month, out hitsCount, page, pageSize);
                string opco = Util.OpcoType();

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
                               industry = item.EventIndustries == null ? string.Empty : GetIndustry(item.EventIndustries),
                               location = item.EventLocation == null ? string.Empty : item.EventLocation,
                               duration = (item.EventStartDate == null && item.EventEndDate == null) ? string.Empty : item.LocalisedEventStartDate.DateRange(item.LocalisedEventStartDate.DateTime, item.LocalisedEventEndDate.DateTime),
                               banner = item.TeaserImage == null ? string.Empty : $"/{opco}{Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(string.Format("{0}", item.TeaserImage.Url))}",
                               link = $"/{opco}{item.Url}"
                           },
                    
                });
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get events listing."), ex);
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

        public IEnumerable<EventList> GetEventsListing(int startIndex, int count)
        {
            try
            {
                int hitsCount;
                var searchResults = SearchEvents(null, null, null, null, out hitsCount, startIndex, count);

                IEnumerable<EventList> eventList = new List<EventList>();
                eventList = searchResults.Select(i => new EventList()
                {
                    EventTitle = i.TeaserTitle,
                    LocalisedEventStartDate = i.LocalisedEventStartDate,
                    LocalisedEventEndDate = i.LocalisedEventEndDate,
                    EventStartDate = i.EventStartDate,
                    EventEndDate = i.EventEndDate,
                    EventUrl = i.Url
                });

                return eventList;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Failed to get automatic events listing."), ex);
                Log.Error(ex.StackTrace, ex);
                return new List<EventList>();
            }
        }
	}
}
