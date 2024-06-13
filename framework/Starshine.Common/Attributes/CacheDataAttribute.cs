using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 缓存数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CacheDataAttribute : Attribute
    {
        /// <summary>
        /// 缓存过期时间（分钟）
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 5;
    }
}
