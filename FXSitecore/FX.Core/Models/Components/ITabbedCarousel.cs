using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Models.Components
{
    [SitecoreType(AutoMap = true, TemplateId = "{DD70D56D-5840-4A63-A569-EDA3380FE6DD}")]
    public interface ITabbedCarousel : IBasePageComponentFields
    {
        [SitecoreChildren(InferType = true)]
        IEnumerable<ITabbedCarouselTab> Tabs {get;set;}
    }

    [SitecoreType(AutoMap = true, TemplateId = "{36180EE3-519A-415B-8894-815C15CC86A4}")]
    public interface ITabbedCarouselTab : ISitecoreItem
    {
        string Title { get; set; }

        [SitecoreChildren(InferType = true)]
        IEnumerable<ITabbedCarouselItem> Items { get; set; }
    }

    [SitecoreType(AutoMap = true, TemplateId = "{7BA76C59-DE2A-4A36-8525-ED4E439C27B0}")]
    public interface ITabbedCarouselItem : ISitecoreItem
    {
        string Text { get; set; }
        Image Image { get; set; }
        Link Link { get; set; }
    }
}
