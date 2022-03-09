using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using FX.Core.Utils;
using System.Linq;

namespace FX.Core.Search.ComputedFields
{
	/// <summary>
	/// Computed field for SnS tagging
	/// </summary>
	public class SnSTaxonomyIndexField : IComputedIndexField
	{
		protected virtual object GetFieldValue(Item item)
		{
			Item[] industries;
			Item[] departments;
			Item[] services;
			Item[] businessTypes;
			industries = departments = services = businessTypes = null;
			var tagList = new List<string[]>();

			industries = item.GetMultilistFieldItems(Templates.SnSStandardPage.Fields.Industries);
			if (industries != null && industries.Any())
			{
				tagList.Add(industries.Select(x => x.ID.ToShortID().ToString()).ToArray());

				departments = item.GetMultilistFieldItems(Templates.SnSStandardPage.Fields.Departments);
				if (departments != null && departments.Any())
				{
					tagList.Add(departments.Select(x => x.ID.ToShortID().ToString()).ToArray());

					services = item.GetMultilistFieldItems(Templates.SnSStandardPage.Fields.Services);
					if (services != null && services.Any())
					{
						tagList.Add(services.Select(x => x.ID.ToShortID().ToString()).ToArray());

						businessTypes = item.GetMultilistFieldItems(Templates.SnSStandardPage.Fields.BusinessTypes);
						if (businessTypes != null && businessTypes.Any())
						{
							tagList.Add(businessTypes.Select(x => x.ID.ToShortID().ToString()).ToArray());
						}
					}
				}


				return string.Join(Constants.SnSTaxonomy.TaxonomySeparator,
					Util.CartesianProduct(tagList).Select(tag => string.Join(Constants.SnSTaxonomy.TagSeparator, tag)));
			}
			return null;
		}

		public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
		{
			Item item = indexable as SitecoreIndexableItem;
			if (item == null)
				return null;

			if (!Util.AreEquals(item.TemplateID.Guid, Templates.SnSStandardPage.Id))
				return null;

			return GetFieldValue(item);
		}

		public string FieldName
		{
			get;
			set;
		}

		public string ReturnType
		{
			get;
			set;
		}
	}
}
