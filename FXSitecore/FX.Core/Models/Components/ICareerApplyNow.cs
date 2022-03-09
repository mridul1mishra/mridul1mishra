using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
    [SitecoreType(TemplateId = Templates.CareerApplyNowComponent.Id, AutoMap = true)]
    public interface ICareerApplyNow : IBasePageComponentFields, IMainContent
    {
        [SitecoreField("Apply Now Label")]
        string ApplyNowLabel { get; set; }

        [SitecoreField("Apply Now Link")]
        Link ApplyNowLink { get; set; }

        [SitecoreField("View All Jobs Label")]
        string ViewAllJobsLabel { get; set; }

        [SitecoreField("View All Jobs Link")]
        Link ViewAllJobsLink { get; set; }
    }
}

