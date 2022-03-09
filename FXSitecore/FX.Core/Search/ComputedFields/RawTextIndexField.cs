using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System.Xml;

namespace FX.Core.Search.ComputedFields
{
	public class RawTextIndexField : AbstractComputedIndexField
	{
		public RawTextIndexField() : this(null) { }

		public RawTextIndexField(XmlNode configNode)
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

			return item[TargetFieldName];
		}

		public string TargetFieldName { get; set; }
	}
}
