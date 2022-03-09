using FX.Core.Utils;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FX.Core.Events
{
	public class AutoSortItemHandler
	{
		private Object __lock = new Object();

		protected List<Template> Templates = new List<Template>();

		private Item ListingItem = null;

		private Item GetListingItem(Item item, Template templ)
		{
			if (item == null)
				return null;
			return item.Axes.GetAncestors().Reverse().Where(i => i.TemplateID.ToString() == templ.ListingId).FirstOrDefault();
		}

		public void OnItemSaved(object sender, EventArgs args)
		{
			Item changedItem = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
			if (changedItem == null) return;

			if (Templates == null || !Templates.Any())
				return;

			// only perform on master database
			if (changedItem.Database == null || changedItem.Database.Name.ToLower() != "master")
				return;

			// check template type
			string templateId = changedItem.TemplateID.ToString();
			Template templ = Templates.FirstOrDefault(t => t.Id == templateId);

			if (templ == null)
				return;

			// check if child of media centre
			ListingItem = GetListingItem(changedItem, templ);
			if (ListingItem == null)
				return;

			// file the item to the right folder
			lock (__lock)
			{
				FileItem(changedItem, templ);
			}
		}

		private Item GetTargetItem(TemplateID templateId, Item item, ItemPath[] paths)
		{
			if (paths == null || !paths.Any())
			{
				return item;
			}

			foreach (var path in paths)
			{
				if (string.IsNullOrWhiteSpace(path.Name))
				{
					throw new ApplicationException("Invalid target item name");
				}

				Item folderItem = item.Children.Where(i => i.Name.Equals(path.Name) && i.TemplateID == templateId.ID).FirstOrDefault();
				if (folderItem == null)
				{
					folderItem = item.Add(path.Name, templateId);
					if (path.HasSpecificDisplayName())
					{
						folderItem.Editing.BeginEdit();
						folderItem.Appearance.DisplayName = path.DisplayName;
						folderItem.Editing.EndEdit();
					}
					folderItem.Publish(false);
				}
				item = folderItem;
			}
			return item;
		}

		private void FileItem(Item item, Template templ)
		{
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				try
				{
					// sort articles in folders yyyy mm eg. 2012 9
					DateTime date = Sitecore.DateUtil.IsoDateToDateTime(item[templ.FieldName]);
					if (date.Year < 1900)
						date = DateTime.Now;

					List<ItemPath> pathList = new List<ItemPath>();
					pathList.Add(new ItemPath(date.Year.ToString()));
					if (!templ.IsYearOnly)
					{
						pathList.Add(new ItemPath(date.Month.ToString(), date.ToString("MMMM")));
					}

					// if the item sits in the proper folder, don't sort it
					string path = Sitecore.StringUtil.EnsurePrefix('/', string.Join("/", pathList.Select(p => p.Name)));
					if (item.Paths.FullPath.EndsWith(string.Format("{0}/{1}", path, item.Name)))
						return;

					TemplateID dateFolderId = new TemplateID(new ID(templ.DateFolderId));
					Item target = GetTargetItem(dateFolderId, ListingItem, pathList.ToArray());
					item.MoveTo(target);
				}
				catch (Exception ex)
				{
					Sitecore.Diagnostics.Log.Error(ex.Message + "\n" + ex.StackTrace, this);
				}
			}
		}

		public void AddTemplates(XmlNode configNode)
		{
			Template templ = CreateTemplate(configNode);
			if (templ != null)
			{
				Templates.Add(templ);
			}
		}

		private Template CreateTemplate(XmlNode node)
		{
			if (!IsValidTemplateNode(node))
				return null;

			Template templ = new Template
			{
				Id = node.Attributes["id"].Value,
				ListingId = node.Attributes["listingId"].Value,
				DateFolderId = IsValidSitecoreID(node.Attributes["folderId"]) ? node.Attributes["folderId"].Value : FX.Core.Templates.NewsSubcategory.Id,
				FieldName = node.Attributes["fieldName"].Value,
				IsYearOnly = node.Attributes["isYearOnly"] == null ? false : "true".Equals(node.Attributes["isYearOnly"].Value, StringComparison.OrdinalIgnoreCase)
			};
			return templ;
		}

		private bool IsValidSitecoreID(string value)
		{
			ID result = ID.Null;
			if (!string.IsNullOrWhiteSpace(value))
			{
				ID.TryParse(value, out result);
			}
			return !ID.IsNullOrEmpty(result);
		}

		private bool IsValidSitecoreID(XmlAttribute attribute)
		{
			return attribute != null && IsValidSitecoreID(attribute.Value);
		}

		private bool IsValidAttribute(XmlAttribute attribute)
		{
			return attribute != null && !string.IsNullOrWhiteSpace(attribute.Value);
		}

		private bool IsValidTemplateNode(XmlNode node)
		{
			return IsValidSitecoreID(node.Attributes["id"]) &&
				IsValidSitecoreID(node.Attributes["listingId"]) &&
				IsValidAttribute(node.Attributes["fieldName"]);
		}
	}

	public class Template
	{
		public string Id;
		public string ListingId;
		public string DateFolderId;
		public string FieldName;
		public bool IsYearOnly;
	}

	public class ItemPath
	{
		public string Name, DisplayName;

		public ItemPath(string name) : this(name, name) { }

		public ItemPath(string name, string displayName)
		{
			Name = name;
			DisplayName = displayName;
		}

		public bool HasSpecificDisplayName()
		{
			return !string.IsNullOrWhiteSpace(DisplayName) &&
				!DisplayName.Equals(Name);
		}
	}
}
