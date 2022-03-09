using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.DiscoverComponent.ID, AutoMap = true)]
    public interface IDiscover : IMicrositeBaseComponentFields
    {
        string Text { get; set; }
        string ButtonText { get; set; }
        Link Link { get; set; }
        Link ButtonLink { get; set; }
    }
}
