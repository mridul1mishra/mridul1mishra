using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components.Accordion
{
	[SitecoreType(TemplateId = Templates.AccordionComponent.Id, AutoMap = true)]
	public interface IAccordion : IBasePageComponentFields
	{
		[SitecoreChildren(InferType = true)]
		IEnumerable<IAccordionItem> AccordionItems { get; set; }
	}
}
