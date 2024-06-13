﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sqlsugar
{
    /// <summary>
    /// 实体种子数据接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISqlSugarEntitySeedData<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> HasData();
    }
}
