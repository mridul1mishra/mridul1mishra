using FX.Core.Models.Base;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace FX.Core.Models.Components
{
	[SitecoreType(TemplateId = Templates.ResourcesComponent.Id, AutoMap = true, EnforceTemplate = SitecoreEnforceTemplate.Template)]
	public interface IResources : IBasePageComponentFields
	{
		IEnumerable<IMediaFile> MediaLink { get; set; }
		string DownloadTitle { get; set; }
		string SubscribeTitle { get; set; }
		string HiddenField { get; set; }
		ISitecoreItem SubscribeForm { get; set; }
	}
}
