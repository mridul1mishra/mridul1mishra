using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.DTO
{
    public class EventList
    {
        public string EventTitle { get; set; }
        public DateTime EventStartDate { get; set; }
        [SitecoreField("EventStartDate")]
        public LocalisedDateTimeValue LocalisedEventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        [SitecoreField("EventEndDate")]
        public LocalisedDateTimeValue LocalisedEventEndDate { get; set; }
        public string EventUrl { get; set; }
    }
}
