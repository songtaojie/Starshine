using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hx.Common
{
    /// <summary>
    /// 分页泛型集合
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PagedListResult<TEntity>
        where TEntity : new()
    {

        /// <summary>
		/// 构造函数
		/// </summary>
		public PagedListResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据集合</param>
        /// <param name="total">总条数</param>
        public PagedListResult(IEnumerable<TEntity> items, int total):this(items,total,1,10)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">数据集合</param>
        /// <param name="total">总条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页条数</param>
        public PagedListResult(IEnumerable<TEntity> items, int total, int page, int pageSize)
        {
            this.Total = total;
            this.Items = items;
            this.Page = page;
            this.PageSize = pageSize;
            var totalPages = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 0;
            this.TotalPages = totalPages;
            HasNextPage = page < totalPages;
            HasPrevPage = page - 1 > 0;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 当前页集合
        /// </summary>
        public IEnumerable<TEntity> Items { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrevPage { get; set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get; set; }
    }
}
