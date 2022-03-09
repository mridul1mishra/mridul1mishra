using FX.Core.GlassMapper.DataHandler;
using FX.Core.Search.Fields;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace FX.Core.Search.Models
{
    public class FXResultItem : SearchResultItem
    {
        [IndexField("_latestversion")]
        public string LatestVersion { get; set; }

        [IndexField("maintitle")]
        public string MainTitle { get; set; }

        [IndexField("teasertitle")]
        public string TeaserTitle { get; set; }

        [IndexField("teaserdescription")]
        public string TeaserDescription { get; set; }

        [IndexField("buttontext")]
        public string ButtonText { get; set; }
        [IndexField("fxteaserimage")]
        public ImageField TeaserImage { get; set; }

        [IndexField("fxsnstaxonomy")]
        public string SnsTaxonomy { get; set; }

        [IndexField("industries")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> Industries { get; set; }

        [IndexField("departments")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> Departments { get; set; }

        [IndexField("services")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> Services { get; set; }

        [IndexField("businesstypes")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> BusinessTypes { get; set; }

        [IndexField("related")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> Related { get; set; }

        [IndexField("countries")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> Countries { get; set; }

        [IndexField("insighttags")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> InsightTags { get; set; }

        [IndexField("parentcategory")]
        public string ParentCategory { get; set; }

        [IndexField("articledate")]
        public System.DateTime ArticleDate { get; set; }

        [IndexField("articledate")]
        public LocalisedDateTimeValue LocalisedArticleDate { get; set; }

        [IndexField("fxcomponentcontent")]
        public string ComponentContent { get; set; }

        [IndexField("metadescription")]
        public string MetaDescription { get; set; }


        #region Products
        [IndexField("ispromotedproduct")]
        public bool IsPromotedProduct { get; set; }

        [IndexField("isbestsellerproduct")]
        public bool IsBestSellerProduct { get; set; }

        [IndexField("productfeatures")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> ProductFeatures { get; set; }

        /// <remarks>
        /// HTML format
        /// </remarks>
        [IndexField("fxproductdescription")]
        public string ProductDescription { get; set; }

        [IndexField("productform")]
        [TypeConverter(typeof(IndexFieldIDValueConverter))]
        public ID ProductForm { get; set; }

        [IndexField("productformlinktext")]
        public string ProductFormLinkText { get; set; }

        [IndexField("fxproductlink")]
        public ButtonLinkField ProductButtonLink { get; set; }
        #endregion

        #region Events
        [IndexField("eventlocations")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> EventLocations { get; set; }

        [IndexField("eventindustries")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> EventIndustries { get; set; }

        [IndexField("eventyear")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> EventYear { get; set; }

        [IndexField("eventmonth")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> EventMonth { get; set; }

        [IndexField("eventstartdate")]
        public System.DateTime EventStartDate { get; set; }

        [IndexField("eventenddate")]
        public System.DateTime EventEndDate { get; set; }

        [IndexField("eventstartdate")]
        public LocalisedDateTimeValue LocalisedEventStartDate { get; set; }

        [IndexField("eventenddate")]
        public LocalisedDateTimeValue LocalisedEventEndDate { get; set; }

        [IndexField("eventlocation")]
        public string EventLocation { get; set; }
        #endregion

        #region Success Story
        [IndexField("successstoriescountries")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> SuccessStoriesCountries { get; set; }

        [IndexField("successstoriessolutions")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> SuccessStoriesSolutions { get; set; }

        [IndexField("successstoriesproducts")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> SuccessStoriesProducts { get; set; }

        [IndexField("successstoriesindustries")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> SuccessStoriesIndustries { get; set; }

        [IndexField("successstoriesyear")]
        [TypeConverter(typeof(IndexFieldEnumerableConverter))]
        public IEnumerable<ID> SuccessStoriesYear { get; set; }

        [IndexField("successstorybuttontext")]
        public string SuccessStoryButtonText { get; set; }
        #endregion

        #region Careers
        [IndexField("specialization")]
        public string Specialization { get; set; }

        [IndexField("careerlocation")]
        public string CareerLocation { get; set; }

        [IndexField("jobpostingdate")]
        public System.DateTime JobPostingDate { get; set; }

        [IndexField("jobpostingdate")]
        public LocalisedDateTimeValue LocalisedJobPostingDate { get; set; }

        #endregion

        [IndexField("grandparent")]
        public ID GrandParent { get; set; }

        [IndexField("insighttype")]
        public string InsightType { get; set; }

        [IndexField("OpCo")]
        public string OpCo { get; set; }
    }
}
