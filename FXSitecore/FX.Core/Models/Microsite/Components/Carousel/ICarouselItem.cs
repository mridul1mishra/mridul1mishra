using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Microsite.Components.Carousel
{
    [SitecoreType(TemplateId = MicrositeTemplates.CarouselItem.ID, AutoMap =true)]
    public interface ICarouselItem : IMicrositeBaseComponentFields
    {
        string Title { get; set; }
        string Description { get; set; }
        string LinkText { get; set; }
        Link Link { get; set; }
        Image Image { get; set; }
    }
}
