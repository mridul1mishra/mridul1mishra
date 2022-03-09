using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.Careers
{
    [SitecoreType(TemplateId = Templates.CareerFilterItem.Id, AutoMap = true)]
    public interface ICareersFilterItem : ISitecoreItem
    {
        [SitecoreField("Career Filter Item Name")]
        string CareerFilterItemName { get; set; }
    }
}
