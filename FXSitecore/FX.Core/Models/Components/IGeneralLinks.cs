using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.GeneralLinksComponent.Id, AutoMap = true)]
	public interface IGeneralLinks : ISitecoreItem
	{
		string Title { get; set; }
		Link Link1 { get; set; }
		Link Link2 { get; set; }
		Link Link3 { get; set; }
		Link Link4 { get; set; }
		Link Link5 { get; set; }
		Link Link6 { get; set; }
	}
}
