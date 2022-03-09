using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Page
{
	public interface IRedirectPage : INavigation, ITeaser
	{
		bool IsPermanentRedirect { get; set; }
		Link RedirectLink { get; set; }
	}
}
