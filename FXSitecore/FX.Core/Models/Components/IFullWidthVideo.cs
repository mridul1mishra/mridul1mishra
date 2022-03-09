using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.TwoColumnCarousel
{
    [SitecoreType(TemplateId = Templates.FullWidthVideoComponent.Id, AutoMap = true)]
    public interface IFullWidthVideo : IBasePageComponentFields
    {
        string Title { get; set; }

        Image Image { get; set; }

        File Video { get; set; }

        Link LinkTo { get; set; }
    }
}
