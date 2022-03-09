using FX.Core.Models.Microsite.Base;
using FX.Core.Models.Microsite.Components.MicrositeListing;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Microsite.Components.MicrositeListing
{
    [SitecoreType(TemplateId = "{097A46A7-7E86-43C6-83D1-451AE8FA01E0}", AutoMap = true)]
    public interface IMicrositeListingImage : IMicrositeListingItem
    {
        Image Image { get; set; }
    }
}
