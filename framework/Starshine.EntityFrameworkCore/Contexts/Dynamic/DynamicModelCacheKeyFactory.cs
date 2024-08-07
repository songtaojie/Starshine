﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 动态模型缓存工厂
    /// </summary>
    /// <remarks>主要用来实现数据库分表分库</remarks>
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        /// <summary>
        /// 动态模型缓存Key
        /// </summary>
        private static int cacheKey;

        /// <summary>
        /// 重写构建模型
        /// </summary>
        /// <remarks>动态切换表之后需要调用该方法</remarks>
        public static void RebuildModels()
        {
            Interlocked.Increment(ref cacheKey);
        }

        /// <summary>
        /// 更新模型缓存
        /// </summary>
        /// <param name="context"></param>
        /// <param name="designTime"></param>
        /// <returns></returns>
        public object Create(DbContext context, bool designTime)
        {

            var dbContextAttribute = DatabaseProviderHelper.GetStarshineDbContextAttribute(context.GetType());

            return dbContextAttribute?.Mode == DbContextMode.Dynamic
                ? (context.GetType(), cacheKey, designTime)
                : context.GetType();
        }
    }
}