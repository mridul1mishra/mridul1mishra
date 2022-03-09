using FX.Core.Models.Base;
using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components.MicrositeBrochureCarousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeBrochureResource.ID, AutoMap = true)]
    public interface IMicrositeBrochureResource : IMicrositeBaseComponentFields
    {
        string Title { get; set; }
        Image Image { get; set; }
        Link Link { get; set; }
        string Description { get; set; }
    }
}
