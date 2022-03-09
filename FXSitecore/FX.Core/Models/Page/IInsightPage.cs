using FX.Core.Models.Base;
using FX.Core.Models.Settings;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Page
{
    public interface IInsightPage : IPage, IMainContent
    {
        string CategoryLabel { get; set; }
        string TypeLabel { get; set; }
        string ResetButtonLabel { get; set; }
        string GridViewLabel { get; set; }
        string ListViewLabel { get; set; }
        string ShowMoreLabel { get; set; }

        

        [SitecoreQuery("./ancestor::*/Content Stores/*[@@templateid='" + Templates.InsightFilterFolder.Id + "']", IsRelative = true, InferType = true)]
        IEnumerable<IInsightFilterFolder> CareersFilterFolder { get; set; }

        IInsightFilterFolder FirstFilter { get; set; }
        IInsightFilterFolder SecondFilter { get; set; }
    }

    public static class IInsightPageExtension 
    {
        public static List<ISitecoreItem> GetFilters(this IInsightPage page)
        {
            List<ISitecoreItem> list = new List<ISitecoreItem>();




            return list;
        }
    }

    [SitecoreType(AutoMap = true, TemplateId = Templates.InsightFilterFolder.Id)]
    public interface IInsightFilterFolder : ISitecoreItem
    {
        [SitecoreChildren(InferType = true)]
        [SitecoreQuery("./*[@@templateid='" + Templates.InsightFilterFolder.Id + "']")]
        IEnumerable<IInsightFilterFolder> SubFolders { get; set; }

        [SitecoreChildren]
        IEnumerable<ITaxonomyItem> Tags { get; set; }
    }
}
