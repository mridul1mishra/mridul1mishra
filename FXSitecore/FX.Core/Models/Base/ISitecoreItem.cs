using FX.Core.Utils;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;

namespace FX.Core.Models.Base
{
	public interface ISitecoreItem
	{
		[SitecoreId]
		Guid Id { get; set; }

		Guid TemplateId { get; set; }

		[SitecoreInfo(SitecoreInfoType.Name)]
		string Name { get; set; }

		[SitecoreInfo(SitecoreInfoType.DisplayName)]
		string DisplayName { get; set; }

		[SitecoreInfo(SitecoreInfoType.Language)]
		string Language { get; set; }

		[SitecoreInfo(SitecoreInfoType.Url)]
		string Url { get; set; }

		[SitecoreInfo(SitecoreInfoType.Path)]
		string Path { get; set; }

		[SitecoreItem]
		Sitecore.Data.Items.Item SitecoreItem { get; set; }
    }

    public static class ISitecoreItemExtension
    {
        public static string URL(this ISitecoreItem item)
        {
            if (Util.AreEquals(item.TemplateId, Core.Templates.RedirectPage.Id))
            {
                var service = new Glass.Mapper.Sc.SitecoreService(Sitecore.Context.Database);
                var redirectPage = service.Cast<FX.Core.Models.Page.IRedirectPage>(item.SitecoreItem);
                if (Util.HasValidLink(redirectPage.RedirectLink, false))
                {
                    return redirectPage.RedirectLink.Url;
                }
            }

            return item.Url;
        }
    }
}
