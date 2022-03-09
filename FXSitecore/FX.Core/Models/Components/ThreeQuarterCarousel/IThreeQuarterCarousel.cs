using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components.ThreeQuarterCarousel
{
    [SitecoreType(TemplateId = Templates.AccordionComponent.Id, AutoMap = true)]
    public interface IThreeQuarterCarousel : IBasePageComponentFields
    {
        string Description { get; set; }
        [SitecoreChildren(InferType = true)]
        IEnumerable<IThreeQuarterCarouselItem> ThreeQuarterCarouselItems { get; set; }
    }
}
