using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Components.MicrositeVideoCarousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeVideoCarousel.ID, AutoMap = true)]
    public interface IMicrositeVideoCarousel : IMicrositeBaseComponentFields
    {
        string Title { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<IMicrositeVideoResource> Videos { get; set; }
    }
}
