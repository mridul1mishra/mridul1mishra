using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace FX.Core.Models.Components.Events
{
    [SitecoreType(TemplateId = Templates.EventOverviewComponent.Id, AutoMap = true)]
    public interface IEventOverview : IBasePageComponentFields, IMainContent
    {
    }
}

