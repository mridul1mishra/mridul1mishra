using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models.Taxonomy
{
	public interface ITaxonomyNode
	{
		string Value { get; set; }
		string Text { get; set; }
	}
}
