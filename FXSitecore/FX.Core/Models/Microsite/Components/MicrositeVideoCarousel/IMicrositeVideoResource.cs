using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components.MicrositeVideoCarousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeVideoResource.ID, AutoMap = true)]
    public interface IMicrositeVideoResource : IMicrositeBaseComponentFields
    {
        string Title { get; set; }
        string YoutubeUrl { get; set; }
        string Description { get; set; }
    }
}
