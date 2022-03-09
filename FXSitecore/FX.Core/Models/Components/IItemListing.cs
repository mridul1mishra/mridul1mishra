using FX.Core.Models.Base;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	public interface IItemListing : ISitecoreItem
	{
		string ListingTitle { get; set; }
		string ListingLinkText { get; set; }
		IStandardPage ListingSource { get; set; }
	}
}
