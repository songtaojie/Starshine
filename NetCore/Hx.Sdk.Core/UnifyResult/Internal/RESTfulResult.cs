namespace Hx.Sdk.Core
{

    /// <summary>
    /// RESTful 风格结果集,泛型结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SkipScan]
    public class RESTfulResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 执行成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 异常码
        /// </summary>
        public object ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
    }
}