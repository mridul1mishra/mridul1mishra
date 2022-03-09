using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeHtmlComponent.ID, AutoMap = true)]
    public interface IHtmlComponent
    {
        string HTML { get; set; }
    }
}
