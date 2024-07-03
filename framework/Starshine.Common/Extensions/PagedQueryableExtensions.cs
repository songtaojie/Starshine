using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Starshine.Extensions
{
	/// <summary>
	/// IQueryable扩展类
	/// </summary>
	[SkipScan]
	public static class PagedQueryableExtensions
	{
		/// <summary>
		/// 排序并分页
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static PagedListResult<T> ToOrderAndPageList<T>(this IQueryable<T> source, BasePageParam param)
			where T : new()
		{
			if (!string.IsNullOrWhiteSpace(param.SortField))
			{
				source = source.ApplyOrder(param.SortField, param.OrderType);
			}
			return source.ToPageList(param);
		}

		/// <summary>
		/// 排序并分页
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public async static Task<PagedListResult<T>> ToOrderAndPageListAsync<T>(this IQueryable<T> source, BasePageParam param)
	where T : new()
		{
			if (!string.IsNullOrWhiteSpace(param.SortField))
			{
				source = source.ApplyOrder(param.SortField, param.OrderType);
			}
			return await source.ToPageListAsync(param);
		}

		/// <summary>
		/// 排序
		/// </summary>
		/// <typeparam name="T">源数据</typeparam>
		/// <param name="source"></param>
		/// <param name="fieldName">排序的字段名称</param>
		/// <param name="orderType">排序的类型</param>
		/// <returns></returns>
		public static IQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string fieldName, OrderTypeEnum? orderType)
		{
			// 升序 or 降序
			string methodName = orderType == OrderTypeEnum.DESC ? "OrderByDescending" : "OrderBy";
			// 属性
			var orderField = string.Empty;
			Type type = typeof(T);
			var properties = type.GetProperties();
			Type[] types = new Type[2];  //  参数：对象类型，属性类型
			types[0] = typeof(T);
			var isMatch = false;
			foreach (var p in properties)
			{
				isMatch = string.Equals(fieldName, p.Name, StringComparison.OrdinalIgnoreCase);
				if (isMatch)
				{
					orderField = p.Name;
					types[1] = p.PropertyType;
					break;
				}
			}
			if (!isMatch) throw new Exception(string.Format("This Sort field 【{0}】 does not exist", fieldName));
			ParameterExpression param = Expression.Parameter(type, orderField);
			Expression orderFieldExp = Expression.Property(param, orderField);
			Expression expr = Expression.Call(typeof(Queryable), methodName, types,
					source.Expression, Expression.Lambda(orderFieldExp, param));
			return source.Provider.CreateQuery<T>(expr);
		}


		/// <summary>
		/// 分页查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="param">分页的参数</param>
		/// <returns></returns>
		public static PagedListResult<T> ToPageList<T>(this IQueryable<T> source, BasePageParam param)
			where T : new()
		{
			if (param == null) throw new ArgumentNullException("param is null");
			return source.ToPageList(param.Page, param.PageSize);
		}

		/// <summary>
		/// 异步分页查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="param">分页参数</param>
		/// <returns></returns>
		public static async Task<PagedListResult<T>> ToPageListAsync<T>(this IQueryable<T> source, BasePageParam param)
			where T : new()
		{
			if (param == null) throw new ArgumentNullException("param is null");
			return await source.ToPageListAsync(param.Page, param.PageSize);
		}

		/// <summary>
		/// 分页查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="pageIndex">当前页码</param>
		/// <param name="pageSize">每页显示的数据条数</param>
		/// <returns></returns>
		public static PagedListResult<T> ToPageList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
			where T : new()
		{
			if (source == null) throw new ArgumentNullException("source is null");
			if (pageIndex <= 0)
			{
				pageIndex = 1;
			}
			if (pageSize <= 0)
			{
				pageSize = 10;
			}
			int totalCount = source.Count<T>();
			if (pageIndex * pageSize > totalCount)
			{
				pageIndex = totalCount / pageSize;
				pageIndex += ((totalCount % pageSize == 0) ? 0 : 1);
				if (pageIndex < 1)
				{
					pageIndex = 1;
				}
			}
			if (totalCount > 0)
			{
				return new PagedListResult<T>(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<T>(), totalCount, pageIndex, pageSize);
			}
			return new PagedListResult<T>(new List<T>(), totalCount, pageIndex, pageSize);
		}

		/// <summary>
		/// 异步分页查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static async Task<PagedListResult<T>> ToPageListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
			where T : new()
		{
			return await Task.FromResult(source.ToPageList(pageIndex, pageSize));
		}
	}
}
