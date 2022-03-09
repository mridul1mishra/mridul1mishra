using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.Website.Models.Shared
{
    public class SocialMediaMetaTag
    {
        public string Property { get; set; }
        public string Content { get; set; }
    }
    public class HeadViewModel
    {
	
		public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MainTitle { get; set; }
        public string CanonicalUrl { get; set; }
        public IEnumerable<SocialMediaMetaTag> SocialMediaTags { get; set; }
        public string Language { get; set; }
    }
}