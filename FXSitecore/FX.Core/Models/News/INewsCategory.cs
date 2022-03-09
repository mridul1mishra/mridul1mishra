using FX.Core.Models.Base;
using FX.Core.Models.Page;
using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace FX.Core.Models.News
{
	public interface INewsCategory : ISitecoreItem
	{
		[SitecoreChildren(InferType = true)]
		IEnumerable<IStandardPage> NewsItems { get; set; }
	}
}
