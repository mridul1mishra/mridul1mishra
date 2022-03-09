using FX.Core.GlassMapper.DataHandler;
using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	public interface IFeaturedContent : IBasePageComponentFields
    {
		string FeaturedTitle1 { get; set; }
		Image FeaturedImage1 { get; set; }
		MultiLineText FeaturedDescription1 { get; set; }
		Link FeaturedLink1 { get; set; }
        string FeaturedLinkText1 { get; set; }

        string FeaturedTitle2 { get; set; }
		Image FeaturedImage2 { get; set; }
		MultiLineText FeaturedDescription2 { get; set; }
		Link FeaturedLink2 { get; set; }
        string FeaturedLinkText2 { get; set; }

        string FeaturedTitle3 { get; set; }
		Image FeaturedImage3 { get; set; }
		MultiLineText FeaturedDescription3 { get; set; }
		Link FeaturedLink3 { get; set; }
        string FeaturedLinkText3 { get; set; }
    }
}
