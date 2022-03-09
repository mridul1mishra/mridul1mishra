using FX.Core.Models.Base;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components.Accordion
{
	public interface IAccordionItem : IMainContent
	{
		string Title { get; set; }
		Image Image { get; set; }
	}
}
