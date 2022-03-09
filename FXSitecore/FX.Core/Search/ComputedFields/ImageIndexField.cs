using FX.Core.Utils;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System.Xml;

namespace FX.Core.Search.ComputedFields
{
	public class ImageIndexField : AbstractComputedIndexField
	{
		public ImageIndexField() : this(null) { }

		public ImageIndexField(XmlNode configNode)
			: base(configNode)
		{
			TargetFieldName = XmlUtil.GetAttribute("targetFieldName", configNode);
		}

		public override object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
		{
			Item item = indexable as SitecoreIndexableItem;
			if (item == null)
				return null;

			if (string.IsNullOrEmpty(TargetFieldName))
				return null;

			ImageField field = item.Fields[TargetFieldName];
			if (field == null || field.MediaItem == null)
				return null;

			return string.Format("{0}|{1}", field.MediaItem.ItemUrl("website") , field.Alt);
		}

		public string TargetFieldName { get; set; }
	}
}
