using Starshine.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Profiling.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///  数据库服务拓展
    /// </summary>
    public static class DatabaseProviderServiceCollectionExtensions
    {

        /// <summary>
        /// 添加默认数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder">自定义配置</param>
        /// <param name="poolSize">设置池保留的最大实例数,默认值为100</param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContextPool<TDbContext>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions> optionBuilder, int poolSize = 100)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            // 注册数据库上下文
            return builder.AddStarshineDbContextPool<TDbContext, DefaultDbContextTypeProvider>(optionBuilder,poolSize);
        }

        /// <summary>
        /// 添加其他数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextProvider">数据库上下文定位器</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder">自定义配置</param>
        /// <param name="poolSize">设置池保留的最大实例数,默认值为100</param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContextPool<TDbContext, TDbContextProvider>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions>? optionBuilder = default,int poolSize = 100)
            where TDbContext : StarshineDbContext<TDbContext>
            where TDbContextProvider : class, IDbContextTypeProvider
        {
           
            // 避免重复注册默认数据库上下文
            DbContextHelper.CheckExistDbContextProvider(typeof(TDbContextProvider));
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext, TDbContextProvider>();
            builder.Services.AddDbContextPool<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                var starshineOptions = new StarshineDbContextOptions(options);
                optionBuilder?.Invoke(starshineOptions);
                if (string.IsNullOrWhiteSpace(starshineOptions.ConnectionString))
                {
                    var dbContextProvider = provider.GetRequiredService<IDbContextProvider>();
                    starshineOptions.ConnectionString = dbContextProvider.GetConnectionString<TDbContext>();
                }
                options.UseDatabase<TDbContext>(starshineOptions);
            }), poolSize);
            return builder;
        }


        /// <summary>
        /// 注册默认数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">StarshineEfCore服务提供器</param>
        public static IStarshineEfCoreBuilder ReplaceDbContext<TDbContext>(this IStarshineEfCoreBuilder builder)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            return builder.ReplaceDbContext<TDbContext, DefaultDbContextTypeProvider>();
        }

        /// <summary>
        /// 替换或注册数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextProvider">数据库上下文定位器</typeparam>
        /// <param name="builder">StarshineEfCore服务提供器</param>
        public static IStarshineEfCoreBuilder ReplaceDbContext<TDbContext, TDbContextProvider>(this IStarshineEfCoreBuilder builder)
            where TDbContext : StarshineDbContext<TDbContext>
            where TDbContextProvider : class, IDbContextTypeProvider
        {
            // 存储数据库上下文和定位器关系
            DbContextHelper.AddOrUpdateDbContextProvider<TDbContext, TDbContextProvider>();
            // 注册数据库上下文
            builder.Services.TryAddScoped<TDbContext>();

            return builder;
        }


        /// <summary>
        ///  添加默认数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder"></param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions>? optionBuilder = default)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            // 注册数据库上下文
            return builder.AddStarshineDbContext<TDbContext, DefaultDbContextTypeProvider>(optionBuilder);
        }

        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextProvider">数据库上下文定位器</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder"></param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext, TDbContextProvider>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions>? optionBuilder = default)
            where TDbContext : StarshineDbContext<TDbContext>
            where TDbContextProvider : class, IDbContextTypeProvider
        {
            // 避免重复注册默认数据库上下文
            DbContextHelper.CheckExistDbContextProvider(typeof(TDbContextProvider));
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext, TDbContextProvider>();
            // 配置数据库上下文
            builder.Services.AddDbContext<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                var starshineOptions = new StarshineDbContextOptions(options);
                optionBuilder?.Invoke(starshineOptions);
                if (string.IsNullOrWhiteSpace(starshineOptions.ConnectionString))
                {
                    var dbContextProvider = provider.GetRequiredService<IDbContextProvider>();
                    starshineOptions.ConnectionString = dbContextProvider.GetConnectionString<TDbContext>();
                }
                options.UseDatabase<TDbContext>(starshineOptions);
            }));

            return builder;
        }
    }
}