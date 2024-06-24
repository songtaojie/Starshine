using Starshine.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库访问器服务拓展类
    /// </summary>
    public static class StarshineEfCoreServiceCollectionExtensions
    {
        private const string DbSettingsOptionsKey = "DbSettings";

        /// <summary>
        /// 添加数据库上下文
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="optionsBuilder">配置</param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineEfCore(this IServiceCollection services, Action<DbSettingsOptions>? optionsBuilder = default)
        {

            ConfigureDbSettingsOptions(services, optionsBuilder);
            // 注册数据库上下文池
            services.TryAddScoped<IDbContextPool, DbContextPool>();
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
                options.Filters.Add<StarshineUnitOfWorkActionFilter>();
            });
            return new StarshineEfCoreBuilder(services)
                .AddStarshineRepositories();
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
            DbContextHelper.CheckDbContextLocator(dbContextLocator, out Type dbContextType);

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
            dbContextPool?.AddToPool(dbContext!);

            return dbContext!;
        }

       

        /// <summary>
        /// 添加 Swagger配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbSettings">配置</param>
        private static void ConfigureDbSettingsOptions(IServiceCollection services, Action<DbSettingsOptions>? dbSettings)
        {
            // 配置验证
            services.AddOptions<DbSettingsOptions>()
                    .BindConfiguration(DbSettingsOptionsKey)
                    .ValidateDataAnnotations()
                    .PostConfigure(options =>
                    {
                        dbSettings?.Invoke(options);
                    });
        }
    }
}