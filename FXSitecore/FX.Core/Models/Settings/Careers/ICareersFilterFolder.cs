using FX.Core.Models.Base;
using FX.Core.Models.Settings.Careers;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace FX.Core.Models.Settings.Careers
{
    [SitecoreType(TemplateId = Templates.CareerFilterFolder.Id, AutoMap = true)]
    public interface ICareersFilterFolder : ISitecoreItem
    {
        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerLocationFilter.Id + "']", IsRelative = true, InferType = true)]
        ICareersLocationFilter CareersLocationFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerSpecialisationFilter.Id + "']", IsRelative = true, InferType = true)]
        ICareersSpecialisationFilter CareersSpecialisationFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerJobTypeFilter.Id + "']", IsRelative = true, InferType = true)]
        ICareersJobTypeFilter CareersJobTypeFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerTitleFilter.Id + "']", IsRelative = true, InferType = true)]
        ICareersTitleFilter CareersTitleFilter { get; set; }

        [SitecoreQuery("./*[@@templateId = '" + Templates.CareerPostingDateFilter.Id + "']", IsRelative = true, InferType = true)]
        ICareersPostingDateFilter CareersPostingDateFilter { get; set; }

        [SitecoreChildren]
        IEnumerable<ICareersFilter> Filters { get; set; }
    }
}
