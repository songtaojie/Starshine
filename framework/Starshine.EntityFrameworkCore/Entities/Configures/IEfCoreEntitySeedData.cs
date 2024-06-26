﻿using Microsoft.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库种子数据依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEfCoreEntitySeedData<TEntity> 
        where TEntity : class
    {
        /// <summary>
        /// 配置种子数据
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <returns></returns>
        IEnumerable<TEntity> HasData(DbContext dbContext);
    }
}