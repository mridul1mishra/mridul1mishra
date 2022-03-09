using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Products
{
	[SitecoreType(TemplateId = Templates.SupplyCategory.Id, AutoMap = true)]
	public interface ISupplyCategory : IProductCategory
	{
	}
}
