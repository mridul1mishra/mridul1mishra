using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.IFrameComponent.Id, AutoMap = true)]
    public interface IIFrame : ISitecoreItem
	{
        [SitecoreField("IFrame URL")]
		string IFrameURL { get; set; }

        [SitecoreField("IFrame Height")]
        int IFrameHeight { get; set; }

        [SitecoreField("IFrame Width")]
        int IFrameWidth { get; set; }
	}
}
