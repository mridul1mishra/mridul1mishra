using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.TakeTheNextStep
{
	[SitecoreType(TemplateId = Templates.EnquiryLinkStep.Id, AutoMap = true)]
	public interface IEnquiryLinkStep : INextStepFields
	{
		Link Link { get; set; }
	}
}
