using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Shared
{
    public class NavigationLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public string Class { get; set; }
    }
}