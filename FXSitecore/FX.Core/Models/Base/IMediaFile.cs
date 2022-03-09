using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Base
{
	public interface IMediaFile : ISitecoreItem
	{
		[SitecoreInfo(SitecoreInfoType.MediaUrl)]
		string MediaUrl { get; set; }

		[SitecoreField("Size")]
		string FileSize { get; set; }
	}
}
