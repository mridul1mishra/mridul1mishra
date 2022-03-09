using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Components.TakeTheNextStep
{
	[SitecoreType(TemplateId = Templates.YoutubeLinkStep.Id, AutoMap = true)]
	public interface IYoutubeLinkStep : INextStepFields
	{
		string YoutubeId { get; set; }
        string VideoUrl { get; set; }
	}
}
