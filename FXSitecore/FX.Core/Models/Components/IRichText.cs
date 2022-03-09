using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.RichTextComponent.Id, AutoMap = true)]
	public interface IRichText : IBasePageComponentFields, IMainContent
	{
	}
}
