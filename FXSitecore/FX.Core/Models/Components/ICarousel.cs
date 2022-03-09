using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	public interface ICarousel : ISitecoreItem
	{
		Image CarouselImage1 { get; set; }
		Image CarouselMobileImage1 { get; set; }
		Link CarouselLink1 { get; set; }

		Image CarouselImage2 { get; set; }
		Image CarouselMobileImage2 { get; set; }
		Link CarouselLink2 { get; set; }

		Image CarouselImage3 { get; set; }
		Image CarouselMobileImage3 { get; set; }
		Link CarouselLink3 { get; set; }

		Image CarouselImage4 { get; set; }
		Image CarouselMobileImage4 { get; set; }
		Link CarouselLink4 { get; set; }

		int CarouselTimer { get; set; }
	}
}
