using FX.Core.Utils;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace FX.Core.Pipelines
{
	public class PageRedirectOption : HttpRequestProcessor
	{
		public override void Process(HttpRequestArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			Item item = Sitecore.Context.Item;
			if (item == null)
				return;

			if (Util.AreEquals(item.TemplateID.Guid, Templates.RedirectPage.Id) && Sitecore.Context.Site.SiteInfo.Name.ToLower() != Sitecore.Constants.ShellSiteName)
			{
				var service = new Glass.Mapper.Sc.SitecoreService(Sitecore.Context.Database);
				var redirectPage = service.Cast<FX.Core.Models.Page.IRedirectPage>(item);
				string url = FXContextItems.HomePage.Url;
				if (Util.HasValidLink(redirectPage.RedirectLink, false))
				{
					url = redirectPage.RedirectLink.Url;
					if (redirectPage.IsPermanentRedirect)
					{
						args.Context.Response.RedirectPermanent(url);
					}
				}
				args.Context.Response.Redirect(url, true);
			}
		}
	}
}
