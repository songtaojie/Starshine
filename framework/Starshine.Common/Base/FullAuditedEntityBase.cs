using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 带有状态数据的实体（泛型）
    /// </summary>
    [SkipScan]
    public abstract class FullAuditedEntityBase<TKey> : AuditedEntityBase<TKey>
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        public virtual TKey DeleterId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeleteTime { get; set; }
    }

    /// <summary>
    /// 带有状态数据的实体（非泛型）
    /// </summary>
    [SkipScan]
    public abstract class FullAuditedEntityBase : AuditedEntityBase
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        public virtual long? DeleterId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime? DeleteTime { get; set; }
    }
}
