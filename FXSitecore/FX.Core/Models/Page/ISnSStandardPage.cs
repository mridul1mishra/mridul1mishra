using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.Page
{
	public interface ISnSStandardPage : IStandardPage
	{
		IEnumerable<ITaxonomyItem> Industries { get; set; }
		IEnumerable<ITaxonomyItem> Departments { get; set; }
		IEnumerable<ITaxonomyItem> Services { get; set; }
		IEnumerable<ITaxonomyItem> BusinessTypes { get; set; }
	}
}
