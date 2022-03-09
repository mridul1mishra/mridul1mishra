using FX.Core.Models.Components;
using FX.Website.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Components
{
    public class Event
    {
        public string EventTitle { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventUrl { get; set; }
    }

    public class LatestEventsViewModel
    {
        public ILatestEvents SitecoreModel { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}