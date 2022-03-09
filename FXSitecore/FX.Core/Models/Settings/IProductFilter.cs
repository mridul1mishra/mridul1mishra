using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using FX.Core.Models.Components.Notifications;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

namespace FX.Core.Models.Settings
{
	public interface IProductFilter : ISitecoreItem
	{
		string FilterControlLabel { get; set; }
		string FilterSelectAllLabel { get; set; }
		ITaxonomyFolder FilterSource { get; set; }
	}
}
