using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models.Taxonomy
{
	public class TaxonomyNode : ITaxonomyNode
	{
		public string Value { get; set; }
		public string Text { get; set; }

		public TaxonomyNode(string value, string text)
		{
			this.Value = value;
			this.Text = text;
		}

		public TaxonomyNode(string text) : this(text, text) { }
	}
}
