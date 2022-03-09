using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreProjectMediaLib.Model
{
    
    [Serializable]
    public class CustomSearchModel : SearchResultItem
    {
        
        [IndexField("blur_b")]
        public string blur { get; set; }

        [IndexField("lighttheme")]
        public string LightTheme { get; set; }
        [IndexField("rightalign")]
        public string RightAlign { get; set; }
        [IndexField("showfollowus")]
        public string showfollowus { get; set; }
        [IndexField("notext")]
        public string NoText { get; set; }
        [IndexField("clear")]
        public string Clear { get; set; }
        [IndexField("narrow")]
        public string Narrow { get; set; }
    }
}