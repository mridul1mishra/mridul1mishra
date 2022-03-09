using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Page
{
    public interface ISnSPage : IPage, IBanner, IMainContent
    {
        [SitecoreField("Load More Label")]
        string LoadMoreLabel { get; set; }

        [SitecoreField("Reset Label")]
        string ResetLabel { get; set; }

        [SitecoreField("Industry Filter Label")]
        string IndustryFilterLabel { get; set; }

        [SitecoreField("Department Filter Label")]
        string DepartmentFilterLabel { get; set; }

        [SitecoreField("Service Filter Label")]
        string ServiceFilterLabel { get; set; }

        [SitecoreField("Business Filter Label")]
        string BusinessFilterLabel { get; set; }

        
    }
}
