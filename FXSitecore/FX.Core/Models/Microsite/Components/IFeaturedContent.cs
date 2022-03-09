using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.FeaturedContentComponent.ID, AutoMap = true)]
    public interface IFeaturedContent : IMicrositeBaseComponentFields
    {
        string Text1 { get; set; }
        string Text2 { get; set; }
        string Text3 { get; set; }

        string LinkText1 { get; set; }
        string LinkText2 { get; set; }
        string LinkText3 { get; set; }

        Link Link1 { get; set; }
        Link Link2 { get; set; }
        Link Link3 { get; set; }
    }
}
