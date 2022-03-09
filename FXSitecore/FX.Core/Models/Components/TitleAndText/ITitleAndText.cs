using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Components.TitleAndText
{
	[SitecoreType(TemplateId = Templates.TitleAndTextComponent.Id, AutoMap = true)]
	public interface ITitleAndText : IBasePageComponentFields
	{
		[SitecoreChildren(InferType = true)]
		IEnumerable<ITitleAndTextItem> TextContent { get; set; }
	}
}
