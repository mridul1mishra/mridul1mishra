using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.RecommendedProducts.Id, AutoMap = true)]
	public interface IRecommendedProducts : ISitecoreItem
	{
		IEnumerable<ISitecoreItem> Products { get; set; }
	}
}
