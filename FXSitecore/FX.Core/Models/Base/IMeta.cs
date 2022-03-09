using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Base
{
	public interface IMeta : ISitecoreItem
	{
		string MetaTitle { get; set; }
		string MetaDescription { get; set; }
		string MetaKeywords { get; set; }

		Image FBImage { get; set; }
		string FBTitle { get; set; }
		string FBDescription { get; set; }

		Image TwitterImage { get; set; }
		string TwitterTitle { get; set; }
		string TwitterDescription { get; set; }
	}
}
