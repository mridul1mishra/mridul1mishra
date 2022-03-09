using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.ChildCardsComponent.Id, AutoMap = true)]
	public interface IChildCards : IBasePageComponentFields
	{
	}
}
