using Starshine.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.Sqlsugar
{
    /// <summary>
    /// 分页拓展类
    /// </summary>
    public static class SqlSugarPagedExtensions
    {
        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="query"><see cref="ISugarQueryable{TEntity}"/>对象</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <param name="expression">查询结果 Select 表达式</param>
        /// <returns></returns>
        public static PagedListResult<TResult> ToPagedList<TEntity, TResult>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize,
            Expression<Func<TEntity, TResult>> expression)
            where TEntity : new()
            where TResult : new()
        {
            var total = 0;
            var items = query.ToPageList(pageIndex, pageSize, ref total, expression);
            return CreateSqlSugarPagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="query"><see cref="ISugarQueryable{TEntity}"/>对象</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <returns></returns>
        public static PagedListResult<TEntity> ToPagedList<TEntity>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize)
            where TEntity : new()
        {
            var total = 0;
            var items = query.ToPageList(pageIndex, pageSize, ref total);
            return CreateSqlSugarPagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="query"><see cref="ISugarQueryable{TEntity}"/>对象</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <param name="expression">查询结果 Select 表达式</param>
        /// <returns></returns>
        public static async Task<PagedListResult<TResult>> ToPagedListAsync<TEntity, TResult>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize,
            Expression<Func<TEntity, TResult>> expression)
            where TEntity : new()
            where TResult : new()
        {
            RefAsync<int> total = 0;
            var items = await query.ToPageListAsync(pageIndex, pageSize, total, expression);
            return CreateSqlSugarPagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="query"><see cref="ISugarQueryable{TEntity}"/>对象</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <returns></returns>
        public static async Task<PagedListResult<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> query, int pageIndex, int pageSize, CancellationToken token = default)
            where TEntity : new()
        {
            RefAsync<int> total = 0;
            var items = await query.ToPageListAsync(pageIndex, pageSize, total, token);
            return CreateSqlSugarPagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页拓展
        /// </summary>
        /// <param name="list">集合对象</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <returns></returns>
        public static PagedListResult<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> list, int pageIndex, int pageSize)
            where TEntity : new()
        {
            var total = list.Count();
            var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return CreateSqlSugarPagedList(items, total, pageIndex, pageSize);
        }

        /// <summary>
        /// 创建 <see cref="PagedListResult{TEntity}"/> 对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="items">分页内容的对象集合</param>
        /// <param name="total">总条数</param>
        /// <param name="pageIndex">当前页码，从1开始</param>
        /// <param name="pageSize">页码容量</param>
        /// <returns></returns>
        private static PagedListResult<TEntity> CreateSqlSugarPagedList<TEntity>(IEnumerable<TEntity> items, int total, int pageIndex, int pageSize) where TEntity : new()
        {
            var totalPages = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;
            return new PagedListResult<TEntity>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Items = items,
                Total = total,
                TotalPages = totalPages,
                HasNextPage = pageIndex < totalPages,
                HasPrevPage = pageIndex - 1 > 0
            };
        }
    }
}
