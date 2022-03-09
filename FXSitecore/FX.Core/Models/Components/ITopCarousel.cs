using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components
{
    public interface ITopCarousel : IBasePageComponentFields
    {
        [SitecoreChildren]
        IEnumerable<ICarouselItem> CarouselItems { get; set; }
    }

    public interface ICarouselItem : ISitecoreItem
    {
        string FeaturedTitle { get; set; }
        Image FeaturedImage { get; set; }
        string FeaturedDescription { get; set; }
        Link FeaturedLink { get; set; }
    }
}
