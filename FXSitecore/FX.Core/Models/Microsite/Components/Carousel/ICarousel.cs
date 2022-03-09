using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Components.Carousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.CarouselComponent.ID, AutoMap = true)]
    public interface ICarousel : IMicrositeBaseComponentFields
    {
        [SitecoreChildren(InferType = true)]
        IEnumerable<ICarouselItem> CarouselItems { get; set; }
    }
}
