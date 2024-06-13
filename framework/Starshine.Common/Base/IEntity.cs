using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [SkipScan]
    public class IEntity<TKey>
    {
        /// <summary>
        /// 主键id
        /// </summary>
        TKey Id { get; set; }
    }
}
