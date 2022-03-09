using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Base
{
	public interface ITeaser : ISitecoreItem
	{
		string TeaserTitle { get; set; }
		MultiLineText TeaserDescription { get; set; }
		Image TeaserImage { get; set; }
        string ButtonText { get; set; }
	}
}
