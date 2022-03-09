using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models.Taxonomy
{
	public class TreeNode<T> where T : ITaxonomyNode
	{
		private T data;

		private List<TreeNode<T>> children = new List<TreeNode<T>>();

		public TreeNode(T data)
		{
			Assert.ArgumentNotNull(data, "data");
			this.data = data;
		}

		public void Add(TreeNode<T> node)
		{
			if (node != null)
			{
				this.children.Add(node);
			}
		}

		public TreeNode<T> FindNodeByValue(string value)
		{
			return this.children.FirstOrDefault(x => string.Compare(x.Value, value, true) == 0);
		}

		public string Value { get { return this.data.Value; } }

		public string Text { get { return this.data.Text; } }

		public TreeNode<T>[] Nodes
		{
			get
			{
				return this.children.ToArray();
			}
		}
	}
}
