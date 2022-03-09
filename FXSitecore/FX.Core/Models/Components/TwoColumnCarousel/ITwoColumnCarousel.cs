using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components.TwoColumnCarousel
{
    [SitecoreType(TemplateId = Templates.AccordionComponent.Id, AutoMap = true)]
    public interface ITwoColumnCarousel : IBasePageComponentFields
    {
        [SitecoreChildren(InferType =true)]
        IEnumerable<ITwoColumnCarouselItem> TwoColumnCarouselItems { get; set; }
    }
}
