using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
    public interface IPromotionListingPage : IBanner, IPage
    {
        [SitecoreField("Show Banner")]
        bool ShowBannerCheckbox { get; set; }

        [SitecoreField("View Detail Label")]
        string ViewDetailLabel { get; set; }

        [SitecoreField("Expired Button Label")]
        string ExpiredButtonLabel { get; set; }

        [SitecoreField("Show View Detail Expired Button Checkbox")]
        bool ShowViewDetailExpiredButtonCheckbox { get; set; }

        [SitecoreField("Promotion Title")]
        string PromotionTitle { get; set; }

        [SitecoreField("Promotion Title Description")]
        string PromotionTitleDescription { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.PromotionDetailPage.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IPromotionDetailPage> PromotionDetailPages { get; set; }
    }
}
