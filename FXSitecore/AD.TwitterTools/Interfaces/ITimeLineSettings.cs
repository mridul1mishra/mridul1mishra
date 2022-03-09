using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.TwitterTools.Interfaces
{
    public interface ITimeLineSettings
    {
        int Count { get; set; }
        string IncludeRts { get; set; }
        string ScreenName { get; set; }
        string TimelineFormat { get; set; }
        string TimelineUrl { get; }
    }
}
