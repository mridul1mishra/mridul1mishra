using FX.Core.Models.Microsite.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;


namespace FX.Core.Models.Microsite.Components.FeaturedLinks
{
    [SitecoreType(TemplateId = MicrositeTemplates.FeaturedLinkComponent.ID, AutoMap = true)]
    public interface IFeaturedLink : IMicrositeBaseComponentFields, IMicrositeHoverColoursField
    {
        string Title { get; set; }
        string Description { get; set; }
        [SitecoreChildren(InferType = true)]
        IEnumerable<IFeaturedRow> FeaturedRows { get; set; }
    }
}
