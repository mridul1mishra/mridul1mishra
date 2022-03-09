using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.TakeTheNextStep
{
	[SitecoreType(TemplateId = Templates.TimeFrameLinkStep.Id, AutoMap = true)]
	public interface ITimeFrameLinkStep : INextStepFields
	{
		string PopupScript { get; set; }
		TimeSlotValue TimeSlot { get; set; }
		MultiLineText ClosedSignText { get; set; }
	}
}
