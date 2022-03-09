using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeIFrameComponent.ID, AutoMap = true)]
    public interface IIFrameMicrositeComponent : IMicrositeBaseComponentFields
	{
        [SitecoreField("IFrame Microsite URL")]
		string IFrameMicrositeURL { get; set; }

        [SitecoreField("IFrame Microsite Height")]
        int IFrameMicrositeHeight { get; set; }

        [SitecoreField("IFrame Microsite Width")]
        int IFrameMicrositeWidth { get; set; }
	}
}
