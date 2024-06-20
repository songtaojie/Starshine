﻿using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using System;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库公开类
    /// </summary>
    public static class Db
    {
        /// <summary>
        /// 未找到服务错误消息
        /// </summary>
        private const string NotFoundServiceErrorMessage = "{0} Service not registered or uninstalled.";

        /// <summary>
        /// 获取非泛型仓储
        /// </summary>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static IRepository GetRepository(IServiceProvider scoped = default)
        {
            return DbContextHelper.GetService<IRepository>(scoped)
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository)));
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="scoped"></param>
        /// <returns>IRepository{TEntity}</returns>
        public static IRepository<TEntity> GetRepository<TEntity>(IServiceProvider scoped = default)
            where TEntity : class, IPrivateEntity, new()
        {
            return DbContextHelper.GetService<IRepository<TEntity>>(scoped)
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository<TEntity>)));
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="scoped"></param>
        /// <returns>IRepository{TEntity, TDbContextLocator}</returns>
        public static IRepository<TEntity, TDbContextLocator> GetRepository<TEntity, TDbContextLocator>(IServiceProvider scoped = default)
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return DbContextHelper.GetService<IRepository<TEntity, TDbContextLocator>>(scoped)
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository<TEntity, TDbContextLocator>)));
        }

        /// <summary>
        /// 获取Sql仓储
        /// </summary>
        /// <param name="scoped"></param>
        /// <returns>ISqlRepository</returns>
        public static ISqlRepository GetSqlRepository(IServiceProvider scoped = default)
        {
            return DbContextHelper.GetService<ISqlRepository>(scoped)
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(ISqlRepository)));
        }

        /// <summary>
        /// 获取Sql仓储
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="scoped"></param>
        /// <returns>ISqlRepository{TDbContextLocator}</returns>
        public static ISqlRepository<TDbContextLocator> GetSqlRepository<TDbContextLocator>(IServiceProvider scoped = default)
            where TDbContextLocator : class, IDbContextLocator
        {
            return DbContextHelper.GetService<ISqlRepository<TDbContextLocator>>(scoped)
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(ISqlRepository<TDbContextLocator>)));
        }

        /// <summary>
        /// 获取作用域数据库上下文
        /// </summary>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetDbContext(IServiceProvider scoped = default)
        {
            return GetDbContext(typeof(DefaultDbContextProvider), scoped);
        }

        /// <summary>
        /// 获取作用域数据库上下文
        /// </summary>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetDbContext(Type dbContextLocator, IServiceProvider scoped = default)
        {
            // 判断是否注册了数据库上下文
            // 判断数据库上下文定位器是否绑定
            DbContextHelper.CheckDbContextLocator(dbContextLocator, out _);

            var dbContextResolve = DbContextHelper.GetService<Func<Type, DbContext>>(scoped);
            return dbContextResolve(dbContextLocator);
        }

        /// <summary>
        /// 获取作用域数据库上下文
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetDbContext<TDbContextLocator>(IServiceProvider scoped = default)
            where TDbContextLocator : class, IDbContextLocator
        {
            return GetDbContext(typeof(TDbContextLocator), scoped);
        }

        /// <summary>
        /// 获取新的瞬时数据库上下文
        /// </summary>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetNewDbContext(IServiceProvider scoped = default)
        {
            return GetNewDbContext(typeof(DefaultDbContextProvider), scoped);
        }

        /// <summary>
        /// 获取新的瞬时数据库上下文
        /// </summary>
        /// <param name="dbContextLocator"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetNewDbContext(Type dbContextLocator, IServiceProvider scoped = default)
        {
            // 判断是否注册了数据库上下文
            // 判断数据库上下文定位器是否绑定
            DbContextHelper.CheckDbContextLocator(dbContextLocator, out _);

            var dbContextResolve = DbContextHelper.GetService<Func<Type, DbContext>>(scoped);
            return dbContextResolve(dbContextLocator);
        }

        /// <summary>
        /// 获取新的瞬时数据库上下文
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static DbContext GetNewDbContext<TDbContextLocator>(IServiceProvider scoped = default)
            where TDbContextLocator : class, IDbContextLocator
        {
            return GetNewDbContext(typeof(TDbContextLocator), scoped);
        }
    }
}
