using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Sitecore.Links;
namespace FX.Core.Utils
{
	public static class ItemExtension
	{
		/// <summary>
		/// This extension method will be used to 
		/// get MultilistField from provided field.
		/// Useful for : Checklist, Multilist, Treelist, Treelist-Ex
		/// </summary>
		/// <param name="item">Item instance</param>
		/// <param name="fieldName">Name of the field</param>
		/// <returns>value of the field</returns>
		public static MultilistField GetMultilistField(this Item item, String fieldName)
		{
			MultilistField multilistField = item.Fields[fieldName];
			return multilistField;
		}

		/// <summary>
		/// This extension method will be used to 
		/// get MultilistField items from provided field.
		/// Useful for : Checklist, Multilist, Treelist, Treelist-Ex
		/// </summary>
		/// <param name="item">Item instance</param>
		/// <param name="fieldName">Name of the field</param>
		/// <returns>value of the field</returns>
		public static Item[] GetMultilistFieldItems(this Item item, String fieldName)
		{
			MultilistField multilistField = item.Fields[fieldName];
			if ((multilistField != null) && (multilistField.InnerField != null))
				return multilistField.GetItems();

			return new Item[] { };
		}

		public static string Url(this Item item)
		{
			if (item != null)
				return Sitecore.Links.LinkManager.GetItemUrl(item);
			else
				return string.Empty;
		}

        public static string Url(this Item item, string language)
        {
            if (item != null)
            {
                UrlOptions defaultUrlOptions = LinkManager.GetDefaultUrlOptions();
                defaultUrlOptions.Language = Sitecore.Globalization.Language.Parse(language);
                return LinkManager.GetItemUrl(item, defaultUrlOptions);
            }
            else
                return string.Empty;
        }

        public static string ItemUrl(this Item item, string siteName)
		{
			if (item == null)
				return null;

			if (item.Paths.IsMediaItem)
			{
				using (new Sitecore.Sites.SiteContextSwitcher(Sitecore.Sites.SiteContext.GetSite(siteName)))
				{
					return Sitecore.Resources.Media.MediaManager.GetMediaUrl(item);
				}
			}
			else
			{
				using (new Sitecore.Sites.SiteContextSwitcher(Sitecore.Sites.SiteContext.GetSite(siteName)))
				{
					return item.Url();
				}
			}
		}

		public static string GetLinkFieldUrl(this Item item, string field)
		{
			if (item == null)
				return string.Empty;

			Sitecore.Data.Fields.LinkField lf = item.Fields[field];

			if (lf == null)
				return string.Empty;

			switch (lf.LinkType.ToLower())
			{
				case "internal":
				case "media":
					return lf.TargetItem.ItemUrl("website");
				case "external":
					// Just return external links
					return lf.Url;
				case "anchor":
					// Prefix anchor link with # if link if not empty
					return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
				case "mailto":
					// Just return mailto link
					return lf.Url;
				case "javascript":
					// Just return javascript
					return lf.Url;
				default:
					// Just please the compiler, this
					// condition will never be met
					return lf.Url;
			}
		}

		public static void Publish(this Item item, bool deep)
		{
			Sitecore.Publishing.PublishOptions publishOptions =
			  new Sitecore.Publishing.PublishOptions(item.Database,
													 Sitecore.Data.Database.GetDatabase("web"),
													 Sitecore.Publishing.PublishMode.SingleItem,
													 item.Language,
													 System.DateTime.Now);

			Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
			publisher.Options.RootItem = item;
			publisher.Options.Deep = deep;
			publisher.Publish();
		}

		public static Sitecore.Layouts.RenderingReference[] GetRenderingReferences(this Item i)
		{
			if (i == null)
			{
				return new Sitecore.Layouts.RenderingReference[0];
			}
			return i.Visualization.GetRenderings(Sitecore.Context.Device, false);
		}

		public static Item[] GetDataSourceItems(this Item i)
		{
			List<Item> list = new List<Item>();
			foreach (Sitecore.Layouts.RenderingReference reference in i.GetRenderingReferences())
			{
				Item dataSourceItem = reference.GetDataSourceItem();
				if (dataSourceItem != null)
				{
					list.Add(dataSourceItem);
				}
			}
			return list.ToArray();
		}

		public static Item GetDataSourceItem(this Sitecore.Layouts.RenderingReference reference)
		{
			if (reference != null)
			{
				return GetDataSourceItem(reference.Settings.DataSource, reference.Database);
			}
			return null;
		}

		private static Item GetDataSourceItem(string id, Sitecore.Data.Database db)
		{
			Guid itemId;
			return Guid.TryParse(id, out itemId)
									? db.GetItem(new Sitecore.Data.ID(itemId))
									: db.GetItem(id);
		}

		public static Item[] GetPersonalizationDataSourceItems(this Item i)
		{
			List<Item> list = new List<Item>();
			foreach (Sitecore.Layouts.RenderingReference reference in i.GetRenderingReferences())
			{
				list.AddRange(reference.GetPersonalizationDataSourceItem());
			}
			return list.ToArray();
		}

		private static Item[] GetPersonalizationDataSourceItem(this Sitecore.Layouts.RenderingReference reference)
		{
			List<Item> list = new List<Item>();
			if (reference != null && reference.Settings.Rules != null && reference.Settings.Rules.Count > 0)
			{
				foreach (var r in reference.Settings.Rules.Rules)
				{
					foreach (var a in r.Actions)
					{
						var setDataSourceAction = a as Sitecore.Rules.ConditionalRenderings.SetDataSourceAction<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext>;
						if (setDataSourceAction != null)
						{
							Item dataSourceItem = GetDataSourceItem(setDataSourceAction.DataSource, reference.Database);
							if (dataSourceItem != null)
							{
								list.Add(dataSourceItem);
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		public static string GetPersonalizationDataSourceItem(this Sitecore.Layouts.RenderingReference reference, Item propertyPageContext)
		{
			List<Item> list = new List<Item>();
			var defaultDataSource = reference.Settings.DataSource; // Default DataSource
			if (reference != null && reference.Settings.Rules != null && reference.Settings.Rules.Count > 0)
			{
				Sitecore.Rules.RuleList<Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext> rules = reference.Settings.Rules;
				if (rules.Count != 0)
				{
					List<Sitecore.Layouts.RenderingReference> references = new List<Sitecore.Layouts.RenderingReference>();
					references.Add(reference);
					var context = new Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext(references, reference);
					context.Item = propertyPageContext;
					reference.Settings.Rules.RunFirstMatching(context);
					if (!string.IsNullOrEmpty(reference.Settings.DataSource))
					{
						defaultDataSource = reference.Settings.DataSource; // Personalized DataSource
					}
				}
			}
			return defaultDataSource;
		}
    }
}
