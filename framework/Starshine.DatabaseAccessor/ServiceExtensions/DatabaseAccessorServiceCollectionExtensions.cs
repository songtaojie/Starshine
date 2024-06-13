using Starshine.DatabaseAccessor;
using Starshine.DatabaseAccessor.Internal;
using Starshine.DatabaseAccessor.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库访问器服务拓展类
    /// </summary>
    public static class DatabaseAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">配置</param>
        /// <param name="dbSettings">配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDatabaseAccessor(this IServiceCollection services, Action<IServiceCollection> configure = null, Action<DbSettingsOptions> dbSettings = null)
        {
            Penetrates.InternalServices = services;
            ConfigureDbOptions(services, dbSettings);

            // 配置数据库上下文
            configure?.Invoke(services);

            // 注册数据库上下文池
            services.TryAddScoped<IDbContextPool, DbContextPool>();

            // 注册 Sql 仓储
            services.TryAddScoped(typeof(ISqlRepository<>), typeof(SqlRepository<>));

            // 注册 Sql 非泛型仓储
            services.TryAddScoped<ISqlRepository, SqlRepository>();

            // 注册多数据库上下文仓储
            services.TryAddScoped(typeof(IRepository<,>), typeof(EFCoreRepository<,>));

            // 注册泛型仓储
            services.TryAddScoped(typeof(IRepository<>), typeof(EFCoreRepository<>));

            // 注册主从库仓储
            services.TryAddScoped(typeof(IMSRepository<,>), typeof(MSRepository<,>));
            services.TryAddScoped(typeof(IMSRepository<,,>), typeof(MSRepository<,,>));

            // 注册非泛型仓储
            services.TryAddScoped<IRepository, EFCoreRepository>();

            // 注册多数据库仓储
            services.TryAddScoped(typeof(IDbRepository<>), typeof(DbRepository<>));

            // 解析数据库上下文
            services.AddScoped(provider =>
            {
                DbContext dbContextResolve(Type locator)
                {
                    return ResolveDbContext(provider, locator);
                }
                return (Func<Type, DbContext>)dbContextResolve;
            });

            // 注册全局工作单元过滤器
            services.Configure<AspNetCore.Mvc.MvcOptions>(options =>
            {
                options.Filters.Add<UnitOfWorkFilter>();
            });
            if (Penetrates.DbSettings.EnabledMiniProfiler == true)
            {
                services.AddMiniProfilerService();
            }
            return services;
        }

        /// <summary>
        /// 通过定位器解析上下文
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        private static DbContext ResolveDbContext(IServiceProvider provider, Type dbContextLocator)
        {
            // 判断定位器是否绑定了数据库上下文
            Penetrates.CheckDbContextLocator(dbContextLocator, out var dbContextType);

            // 动态解析数据库上下文
            var dbContext = provider.GetService(dbContextType) as DbContext;

            // 实现动态数据库上下文功能，刷新 OnModelCreating
            var dbContextAttribute = DbProvider.GetAppDbContextAttribute(dbContextType);
            if (dbContextAttribute?.Mode == DbContextMode.Dynamic)
            {
                DynamicModelCacheKeyFactory.RebuildModels();
            }

            // 添加数据库上下文到池中
            var dbContextPool = provider.GetService<IDbContextPool>();
            dbContextPool?.AddToPool(dbContext);

            return dbContext;
        }

        /// <summary>
        /// 注册默认数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <param name="services">服务提供器</param>
        public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            return services.RegisterDbContext<TDbContext, MasterDbContextLocator>();
        }

        /// <summary>
        /// 注册数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <param name="services">服务提供器</param>
        public static IServiceCollection RegisterDbContext<TDbContext, TDbContextLocator>(this IServiceCollection services)
            where TDbContext : DbContext
            where TDbContextLocator : class, IDbContextLocator
        {
            // 存储数据库上下文和定位器关系
            Penetrates.DbContextDescriptors.AddOrUpdate(typeof(TDbContextLocator), typeof(TDbContext), (key, value) => typeof(TDbContext));
            // 注册数据库上下文
            services.TryAddScoped<TDbContext>();

            return services;
        }

        /// <summary>
        /// 添加 Swagger配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbSettings">配置</param>
        private static void ConfigureDbOptions(IServiceCollection services, Action<DbSettingsOptions> dbSettings)
        {
            // 配置验证
            services.AddOptions<DbSettingsOptions>()
                    .BindConfiguration("DbSettings")
                    .ValidateDataAnnotations()
                    .PostConfigure(options =>
                    {
                        dbSettings?.Invoke(options);
                    });
        }
    }
}