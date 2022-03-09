using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;


namespace FX.Core.Models.Microsite.Components.MicrositeListing
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeListingItem.ID, AutoMap = true)]
    public interface IMicrositeListingRow : IMicrositeListingItem, IMicrositeHoverColoursField
    {
        string Title { get; set; }
        string Text { get; set; }
        Image Icon { get; set; }
    }
}
