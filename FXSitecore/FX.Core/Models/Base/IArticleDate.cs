using FX.Core.GlassMapper.DataHandler;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;

namespace FX.Core.Models.Base
{
	public interface IArticleDate : ISitecoreItem
	{
		DateTime ArticleDate { get; set; }

        [SitecoreField("ArticleDate")]
        LocalisedDateTimeValue LocalisedArticleDate { get; set; }
    }
}
