using FX.Core.Models.Microsite.Base;
using FX.Core.Models.Microsite.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Components.MicrositeArticleCarousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeArticleCarousel.ID, AutoMap = true)]
    public interface IMicrositeArticleCarousel : IMicrositeBaseComponentFields
    {
        string Title { get; set; }
        IEnumerable<IMicrositePage> Articles { get; set; }
    }
}
