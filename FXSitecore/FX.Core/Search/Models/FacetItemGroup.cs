using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FX.Core.Search.Models
{
	public class FacetItemGroup<T>
	{
		public string Caption { get; set; }
		public string FacetName { get; set; }
		public Expression<Func<T, object>> Field { get; private set; }
		public Func<string, string> OnDisplayValue { get; set; }
		public IEnumerable<FacetItem<T>> Values { get; set; }
		public List<string> Filter { get; private set; }

		public static PropertyInfo GetPropertyInfo(Expression<Func<T, object>> expression)
		{
			MemberExpression memberExpr = expression.Body as MemberExpression;
			if (memberExpr == null)
			{
				UnaryExpression unaryExpr = expression.Body as UnaryExpression;
				if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
				{
					memberExpr = unaryExpr.Operand as MemberExpression;
				}
			}
			if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
			{
				return memberExpr.Member as PropertyInfo;
			}
			throw new ArgumentException("Invalid property expression");
		}

		public FacetItemGroup(Expression<Func<T, object>> expression) : this(expression, null) { }

		public FacetItemGroup(Expression<Func<T, object>> expression, string caption, params string[] filterValues)
		{
			PropertyInfo propInfo = GetPropertyInfo(expression);
			var attr = propInfo.GetCustomAttribute<Sitecore.ContentSearch.IndexFieldAttribute>();
			if (attr != null)
			{
				FacetName = attr.IndexFieldName;
			}
			else
			{
				FacetName = propInfo.Name;
			}
			Field = expression;

			if (!string.IsNullOrWhiteSpace(caption))
			{
				Caption = caption;
			}

			Filter = new List<string>();
			if (filterValues != null && filterValues.Any())
			{
				Filter.AddRange(filterValues.Where(filter => !string.IsNullOrWhiteSpace(filter)));
			}
		}

		public Expression<Func<T, bool>> CreateFilterExpression(string value)
		{
			PropertyInfo propInfo = GetPropertyInfo(Field);

			var argExpr = Expression.Parameter(typeof(T), "p");
			var propertyExpr = Expression.Property(argExpr, propInfo);
			var constExpr = Expression.Constant(value);

			Expression compExpr = Expression.Equal(propertyExpr, constExpr);

			return Expression.Lambda<Func<T, bool>>(compExpr, argExpr);
		}
	}
}
