using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>: IEntity
    {
        /// <summary>
        /// 主键id
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Returns an array of ordered keys for this entity.
        /// </summary>
        /// <returns></returns>
        object[] GetKeys();
    }
}
