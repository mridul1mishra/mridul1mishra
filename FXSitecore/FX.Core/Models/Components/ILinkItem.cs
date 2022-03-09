using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
    [SitecoreType(TemplateId = Templates.LinkItem.Id, AutoMap = true )]
    public interface ILinkItem  : ISitecoreItem
    {
        Link Link { get; set; }
    }
}
