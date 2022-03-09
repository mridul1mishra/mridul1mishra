using Sitecore.ContentSearch;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models
{
	public class SearchParam<T> where T : FXResultItem
	{
		private int _page;
		private int _pageSize;
		public string IndexName { get; set; }

		public SearchParam(Database db)
		{
			IndexName = string.Format("sitecore_{0}_index", db.Name);
			TemplateRestrictions = new List<ID>();
			this.Facets = new List<FacetItemGroup<T>>();
		}

		public List<ID> TemplateRestrictions { get; private set; }

		public List<FacetItemGroup<T>> Facets { get; private set; }

		public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }

		public Expression<Func<T, bool>> WhereClause { get; set; }

		public int Page
		{
			get
			{
				return _page;
			}
			set
			{
				this._page = Math.Max(1, value);
			}
		}

		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				this._pageSize = Math.Max(0, value);
			}
		}

		public ID ItemPathId { get; set; }

		public void AddTemplateRestriction(ID templateId)
		{
			if (!ID.IsNullOrEmpty(templateId))
			{
				TemplateRestrictions.Add(templateId);
			}
		}

		public void AddFacet(FacetItemGroup<T> facetItemGroup)
		{
			if (facetItemGroup != null)
			{
				Facets.Add(facetItemGroup);
			}
		}

		public string[] GetValidFacets()
		{
			var result = new List<string>();
			if (Facets != null && Facets.Any())
			{
				result.AddRange(Facets.Where(f => f != null && !string.IsNullOrWhiteSpace(f.FacetName)).Select(f => f.FacetName));
			}

			return result.ToArray();
		}
	}
}
