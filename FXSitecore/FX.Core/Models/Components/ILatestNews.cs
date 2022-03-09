using FX.Core.Models.Base;
using FX.Core.Models.News;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	public interface ILatestNews : ISitecoreItem
	{
		string ListingTitle { get; set; }
		string ListingLinkText { get; set; }
		INewsroomPage ListingSource { get; set; }
	}
}
