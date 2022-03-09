using FX.Core.Search.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FX.Core.Utils
{
	public static class SearchHelper
	{
		public static SearchResults<T> GetPagedSearchResults<T>(IQueryable<T> query,
			Expression<Func<T, bool>> whereClause,
			Expression<Func<T, bool>> filterBy,
			string[] facets,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
			int page, int pageSize) where T : SearchResultItem
		{
			int offset = 0;
			if (pageSize > 0)
			{
				page = Math.Max(1, page);
				offset = (page - 1) * pageSize;
			}
			return GetSearchResults<T>(query, whereClause, filterBy, facets, orderBy, offset, pageSize);
		}

		public static SearchResults<T> GetSearchResults<T>(IQueryable<T> query,
			Expression<Func<T, bool>> whereClause,
			Expression<Func<T, bool>> filterBy,
			string[] facets,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int offset, int pageSize) where T : SearchResultItem
		{
			if (whereClause != null)
			{
				query = query.Where(whereClause);
			}
			if (filterBy != null)
			{
				query = query.Filter(filterBy);
			}

			if (facets != null && facets.Any())
			{
				query = facets.Aggregate(query, (current, facetName) => current.FacetOn(c => c[facetName], 1));
			}

			offset = Math.Max(0, offset);
			if (pageSize > 0)
			{
				if (orderBy == null)
				{
					query = query.Skip(offset).Take(pageSize);
				}
				else
				{
					query = orderBy(query).Skip(offset).Take(pageSize);
				}
			}
			else
			{
				if (orderBy != null)
				{
					query = orderBy(query).Skip(offset);
				}
			}

			return query.GetResults();
		}

		public static Expression<Func<T, bool>> CombineAndClauses<T>(params Expression<Func<T, bool>>[] predicates) where T : SearchResultItem
		{
			var result = PredicateBuilder.True<T>();
			foreach (var predicate in predicates)
			{
				if (predicate != null)
				{
					result.And(predicate);
				}
			}
			return result;
		}

		public static Expression<Func<T, bool>> GetTemplateFilterExpression<T>(params Sitecore.Data.ID[] templateRestrictions) where T : SearchResultItem
		{
			var predicate = PredicateBuilder.True<T>();
			if (templateRestrictions != null && templateRestrictions.Any())
			{
				predicate = templateRestrictions.Aggregate(predicate, (current, t) => current.Or(p => p.TemplateId == t));
			}
			return predicate;
		}

		public static IEnumerable<T> Search<T>(SearchParam<T> searchParam, out int hitsCount) where T : FXResultItem
		{
			hitsCount = 0;
			ISearchIndex searchIndex = ContentSearchManager.GetIndex(searchParam.IndexName);
			using (var context = searchIndex.CreateSearchContext())
			{
				var filterBy = PredicateBuilder.True<T>().And(x => "1".Equals(x.LatestVersion));
				if (searchParam.TemplateRestrictions.Any())
				{
					filterBy = filterBy.And(SearchHelper.GetTemplateFilterExpression<T>(searchParam.TemplateRestrictions.ToArray()));
				}

				if (!ID.IsNullOrEmpty(searchParam.ItemPathId))
				{
					filterBy = filterBy.And(x => x.Paths.Contains(searchParam.ItemPathId));
				}

				if (searchParam.Facets != null)
				{
					foreach (var facet in searchParam.Facets)
					{
						foreach (var filter in facet.Filter)
						{
							filterBy = filterBy.And(facet.CreateFilterExpression(filter));
						}
					}
				}

				var searchResults = SearchHelper.GetPagedSearchResults<T>(
					context.GetQueryable<T>(),
					searchParam.WhereClause,
					filterBy,
					searchParam.GetValidFacets(),
					searchParam.OrderBy, searchParam.Page, searchParam.PageSize);
				if (searchResults != null)
				{
					hitsCount = searchResults.TotalSearchResults;

					if (searchParam.Facets != null)
					{
						if (searchResults.Facets != null)
						{
							searchResults.Facets.Categories.ForEach(x =>
							{
								var facet = searchParam.Facets.FirstOrDefault(f => f.FacetName == x.Name);
								if (facet != null)
								{
									facet.Values = x.Values.Select(v => new FacetItem<T>(v.Name, v.AggregateCount, facet.Filter.Contains(v.Name), facet));
								}
							});
						}
					}

					return searchResults.Hits.Select(x => x.Document).ToList();
				}
			}
			return Enumerable.Empty<T>();
		}

		public static ID ParseId(string value)
		{
			ID result = ID.Null;

			ShortID shortId;
			if (ShortID.TryParse(value, out shortId))
			{
				result = shortId.ToID();
			}
			else
			{
				ID.TryParse(value, out result);
			}
			return result;
		}
	}
}
