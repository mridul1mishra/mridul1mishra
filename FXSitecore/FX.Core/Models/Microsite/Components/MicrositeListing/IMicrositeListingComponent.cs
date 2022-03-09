using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Microsite.Components.MicrositeListing
{
    [SitecoreType(TemplateId = MicrositeTemplates.MicrositeListingComponent.ID, AutoMap = true)]
    public interface IMicrositeListingComponent : IMicrositeBaseComponentFields
    {
        [SitecoreChildren(InferType = true)]
        IEnumerable<IMicrositeListingItem> ListingItems { get; set; }
    }
}
