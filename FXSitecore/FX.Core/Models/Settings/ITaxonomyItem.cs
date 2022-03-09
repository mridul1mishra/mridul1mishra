using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Settings
{
	public interface ITaxonomyItem : ISitecoreItem
	{
        [SitecoreField("Taxonomy Item Name")]
        string TaxonomyItemName { get; set; }

        [SitecoreField("Description Line 1")]
        string Description1 { get; set; }
    }
}
