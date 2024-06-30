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
    public static class StarshineDbContextServiceCollectionExtensions
    {

        /// <summary>
        /// 添加其他数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder">自定义配置</param>
        /// <param name="poolSize">设置池保留的最大实例数,默认值为100</param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContextPool<TDbContext>(this IStarshineEfCoreBuilder builder, Action<IStarshineDbContextOptionsBuilder>? optionBuilder = default,int poolSize = 100)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            var starshineOptions = new StarshineDbContextOptions(typeof(TDbContext));
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext>();
            builder.Services.AddDbContextPool<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                optionBuilder?.Invoke(starshineOptions);
                starshineOptions.DbContextOptions?.Invoke(options);
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
        /// 替换或注册数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">StarshineEfCore服务提供器</param>
        public static IStarshineEfCoreBuilder ReplaceDbContext<TDbContext>(this IStarshineEfCoreBuilder builder)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            var targetDbContextType = typeof(TDbContext);
            builder.Services.Replace(ServiceDescriptor.Scoped(targetDbContextType, sp =>
            {
                var dbContextType = sp.GetRequiredService<IDbContextTypeProvider>().GetDbContextType(targetDbContextType);
                return sp.GetRequiredService(dbContextType);
            }));
            builder.Services.Configure<StarshineDbContextOptions>(opts =>
            {
                var dbContextKey = $"{targetDbContextType.FullName}";
                opts.DbContextReplacements[dbContextKey] = targetDbContextType;
            });
            // 注册数据库上下文
            builder.Services.TryAddScoped<TDbContext>();

            return builder;
        }

        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder"></param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions>? optionBuilder = default)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            var starshineOptions = new StarshineDbContextOptions(typeof(TDbContext));
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext>();
            // 配置数据库上下文
            builder.Services.AddDbContext<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
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