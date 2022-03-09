using FX.Core.Utils;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FX.Core.Search.ComputedFields
{
	public class ButtonLinkIndexField : AbstractComputedIndexField
	{
		public ButtonLinkIndexField() : this(null) { }

		public ButtonLinkIndexField(XmlNode configNode)
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

			LinkField field = item.Fields[TargetFieldName];
			if (field == null)
				return null;

			return string.Format("{0}|{1}|{2}", item.GetLinkFieldUrl(TargetFieldName), field.Target, field.Text);
		}

		public string TargetFieldName { get; set; }
	}
}
