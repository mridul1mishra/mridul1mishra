using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models
{
	public class FacetItem<T>
	{
		public string Value { get; set; }
		public int Count { get; set; }
		public bool IsSelected { get; set; }
		public FacetItemGroup<T> FacetCategory { get; private set; }

		public FacetItem(string value, int count, bool isSelected, FacetItemGroup<T> facetCategory)
		{
			this.Value = value;
			this.Count = count;
			this.IsSelected = isSelected;
			this.FacetCategory = facetCategory;
		}

		public string Text
		{
			get
			{
				if (FacetCategory.OnDisplayValue != null)
				{
					return FacetCategory.OnDisplayValue(Value);
				}
				return Value;
			}
		}
	}
}
