using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Microsite.Components
{
    [SitecoreType(TemplateId = MicrositeTemplates.TwoColumnTextFormComponent.ID, AutoMap = true)]
    public interface ITwoColumnTextFormComponent : IMicrositeBaseComponentFields
    {
        string Text { get; set; }
        string Title { get; set; }
        bool FormOnLeft { get; set; }
    }
}
