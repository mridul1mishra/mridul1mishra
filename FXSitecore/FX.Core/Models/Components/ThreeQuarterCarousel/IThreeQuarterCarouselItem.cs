using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.ThreeQuarterCarousel
{
    public interface IThreeQuarterCarouselItem : ISitecoreItem
    {
        string Title { get; set; }

        string Description { get; set; }

        Image Image { get; set; }

        string ButtonText { get; set; }

        Link ButtonLink { get; set; }
    }
}
