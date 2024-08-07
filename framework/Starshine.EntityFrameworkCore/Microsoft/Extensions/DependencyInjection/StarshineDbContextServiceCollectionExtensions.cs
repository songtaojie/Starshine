﻿using Starshine.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            var starshineOptions = new StarshineDbContextOptions(typeof(TDbContext),builder.Services);
            optionBuilder?.Invoke(starshineOptions);
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext>();
            builder.Services.AddDbContextPool<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                if (string.IsNullOrWhiteSpace(starshineOptions.ConnectionString))
                {
                    var connectionStringResolver = provider.GetRequiredService<IConnectionStringResolver>();
                    starshineOptions.ConnectionString = connectionStringResolver.ResolveAsync<TDbContext>().GetAwaiter().GetResult();
                }
                starshineOptions.DbContextOptions?.Invoke(options);
                options.UseDatabase<TDbContext>(starshineOptions);
            }), poolSize);
            starshineOptions.AddDefaultRepositories();
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
            //builder.Services.Replace(ServiceDescriptor.Scoped(targetDbContextType, sp =>
            //{
            //    var dbContextType = sp.GetRequiredService<IDbContextTypeProvider>().GetDbContextType(targetDbContextType);
            //    return sp.GetRequiredService(dbContextType);
            //}));
            builder.Services.Configure<StarshineDbContextTypeOptions>(opts =>
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
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext>(this IStarshineEfCoreBuilder builder, Action<IStarshineDbContextOptionsBuilder>? optionBuilder = default)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            var starshineOptions = new StarshineDbContextOptions(typeof(TDbContext), builder.Services);
            optionBuilder?.Invoke(starshineOptions);
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext>();
            // 配置数据库上下文
            builder.Services.AddDbContext<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                if (string.IsNullOrWhiteSpace(starshineOptions.ConnectionString))
                {
                    var connectionStringResolver = provider.GetRequiredService<IConnectionStringResolver>();
                    starshineOptions.ConnectionString = connectionStringResolver.ResolveAsync<TDbContext>().GetAwaiter().GetResult();
                }
                starshineOptions.DbContextOptions?.Invoke(options);
                options.UseDatabase<TDbContext>(starshineOptions);
            }));
            starshineOptions.AddDefaultRepositories();
            return builder;
        }
    }
}