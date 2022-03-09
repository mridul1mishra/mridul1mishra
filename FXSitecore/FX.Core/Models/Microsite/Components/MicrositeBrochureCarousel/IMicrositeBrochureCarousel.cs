using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Components.MicrositeBrochureCarousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeBrochureCarousel.ID, AutoMap = true)]
    public interface IMicrositeBrochureCarousel : IMicrositeBaseComponentFields
    {
        string Title { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IMicrositeBrochureResource> Brochures { get; set; }
    }
}
