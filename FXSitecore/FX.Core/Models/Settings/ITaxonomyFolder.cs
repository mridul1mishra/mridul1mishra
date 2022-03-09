using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Settings
{
	public interface ITaxonomyFolder : ISitecoreItem
	{
		[SitecoreChildren(InferType = true)]
		IEnumerable<ITaxonomyItem> TaxonomyItems { get; set; }
	}
}
