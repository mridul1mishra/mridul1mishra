using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings.SuccessStories
{
    [SitecoreType(TemplateId = Templates.SuccessStoriesFilterItem.Id, AutoMap = true)]
    public interface ISuccessStoriesFilterItem : ISitecoreItem
    {
        [SitecoreField("Success Story Filter Item Name")]
        string SuccessStoryFilterItemName { get; set; }
    }
}
