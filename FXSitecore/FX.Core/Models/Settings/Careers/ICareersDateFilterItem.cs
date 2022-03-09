using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.Careers
{
    [SitecoreType(TemplateId = Templates.CareerDateFilterItem.Id, AutoMap = true)]
    public interface ICareersDateFilterItem : ISitecoreItem
    {
        [SitecoreField("Career Date Filter Item Name")]
        string CareerDateFilterItemName { get; set; }

        [SitecoreField("Number Of Days")]
        int NumberOfDays { get; set; }
    }
}
