using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Products
{
	[SitecoreType(TemplateId = Templates.SoftwareCategory.Id, AutoMap = true)]
	public interface ISoftwareCategory : IProductCategory
	{
	}
}
