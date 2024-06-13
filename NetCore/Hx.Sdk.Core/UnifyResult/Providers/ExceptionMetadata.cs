using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 异常元数据
    /// </summary>
    [SkipScan]
    public sealed class ExceptionMetadata
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; internal set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public object ErrorCode { get; internal set; }

        /// <summary>
        /// 错误对象（信息）
        /// </summary>
        public object Errors { get; internal set; }

        /// <summary>
        /// 错误对象（信息）
        /// </summary>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Data { get; internal set; }
    }
}
