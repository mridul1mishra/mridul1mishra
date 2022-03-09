using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Products
{
	[SitecoreType(TemplateId = Templates.PrinterCategory.Id, AutoMap = true)]
	public interface IPrinterCategory : IProductCategory
	{
	}
}
