
namespace Hx.Common
{
    /// <summary>
    /// 分页参数
    /// </summary>
    [SkipScan]
    public class BasePageParam
    {
        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 当前页码
        /// 默认从第一页开始
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 排序的字段
        /// </summary>
        public string SortField { get; set; } = string.Empty;
        /// <summary>
        /// 0 正序 1倒序
        /// </summary>
        public OrderTypeEnum? OrderType { get; set; } = OrderTypeEnum.ASC;
    }
    /// <summary>
    /// 排序类型
    /// </summary>
    [SkipScan]
    public enum OrderTypeEnum
    {
        /// <summary>
        /// 正序
        /// </summary>
        ASC = 1,
        /// <summary>
        /// 倒序
        /// </summary>
        DESC = 2
    }
}
