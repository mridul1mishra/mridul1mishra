using AD.TwitterTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.TwitterTools
{
    public class TimeLineSettings : ITimeLineSettings
    {
        public string ScreenName { get; set; }
        public string IncludeRts { get; set; }
        public string ExcludeReplies { get; set; }
        public int Count { get; set; }
        public string TimelineFormat { get; set; }
        public string TimelineUrl
        {
            get
            {
                return string.Format(TimelineFormat, ScreenName, IncludeRts, ExcludeReplies, Count);
            }
        }
    }
}
