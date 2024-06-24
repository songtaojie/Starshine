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
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions> optionBuilder)
            where TDbContext : StarshineDbContext<TDbContext>
        {
            // 注册数据库上下文
            return builder.AddStarshineDbContext<TDbContext, DefaultDbContextTypeProvider>(optionBuilder, optionBuilder);
        }

        /// <summary>
        /// 添加其他数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextProvider">数据库上下文定位器</typeparam>
        /// <param name="builder">服务</param>
        /// <param name="optionBuilder">自定义配置</param>
        /// <returns>服务集合</returns>
        public static IStarshineEfCoreBuilder AddStarshineDbContext<TDbContext, TDbContextProvider>(this IStarshineEfCoreBuilder builder, Action<StarshineDbContextOptions>? optionBuilder = default)
            where TDbContext : StarshineDbContext<TDbContext>
            where TDbContextProvider : class, IDbContextTypeProvider
        {
            var starshineOptions = new StarshineDbContextOptions();
            optionBuilder?.Invoke(starshineOptions);
            // 避免重复注册默认数据库上下文
            DbContextHelper.CheckExistDbContextProvider(typeof(TDbContextProvider));
            // 注册数据库上下文
            builder.ReplaceDbContext<TDbContext, TDbContextProvider>();
            builder.Services.AddDbContextPool<TDbContext>((provider, options) =>
            {
                
            }, poolSize: starshineOptions.PoolSize);

            builder.Services.AddDbContextPool<TDbContext>(DbContextHelper.ConfigureDbContext((provider, options) =>
            {
                if (string.IsNullOrWhiteSpace(starshineOptions.ConnectionString))
                {
                    var dbContextProvider = provider.GetRequiredService<IDbContextProvider>();
                    starshineOptions.ConnectionString = dbContextProvider.GetConnectionString<TDbContext>();
                }

                var _options = ConfigureDatabase<TDbContext>(providerName, starshineOptions.ConnectionString, options);
                starshineOptions.DbContextOptions?.Invoke(_options);
            }), poolSize: starshineOptions.PoolSize);

            return builder;
        }

        /// <summary>
        /// 添加其他数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文</typeparam>
        /// <typeparam name="TDbContextProvider">数据库上下文定位器</typeparam>
        /// <param name="services">服务</param>
        /// <param name="optionBuilder">自定义配置</param>
        /// <param name="poolSize">池大小</param>
        /// <param name="interceptors">拦截器</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStarshineDbContext<TDbContext, TDbContextProvider>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionBuilder, int poolSize = 100, params IInterceptor[] interceptors)
            where TDbContext : StarshineDbContext<TDbContext>
            where TDbContextProvider : class, IDbContextTypeProvider
        {
            // 避免重复注册默认数据库上下文
            DbContextHelper.CheckExistDbContextProvider(typeof(TDbContextProvider));
            // 注册数据库上下文
            services.RegisterDbContext<TDbContext, TDbContextProvider>();

            // 配置数据库上下文
            services.AddDbContextPool<TDbContext>(DbContextHelper.ConfigureDbContext(optionBuilder, interceptors), poolSize: poolSize);
            return services;
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
        /// 注册数据库上下文
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


        ///// <summary>
        /////  添加默认数据库上下文
        ///// </summary>
        ///// <typeparam name="TDbContext">数据库上下文</typeparam>
        ///// <param name="services">服务</param>
        ///// <param name="providerName">数据库提供器</param>
        ///// <param name="optionBuilder"></param>
        ///// <param name="connectionString">连接字符串</param>
        ///// <param name="interceptors">拦截器</param>
        ///// <returns>服务集合</returns>
        //public static IServiceCollection AddDb<TDbContext>(this IServiceCollection services, string? providerName = default, Action<DbContextOptionsBuilder> optionBuilder = null, string connectionString = default, params IInterceptor[] interceptors)
        //    where TDbContext : DbContext
        //{
        //    // 避免重复注册默认数据库上下文
        //    if (DbContextHelper.DbContextDescriptors.ContainsKey(typeof(DefaultDbContextTypeProvider))) 
        //        throw new InvalidOperationException("Prevent duplicate registration of default DbContext.");

        //    // 注册数据库上下文
        //    return services.AddDb<TDbContext, DefaultDbContextTypeProvider>(providerName, optionBuilder, connectionString, interceptors);
        //}

        ///// <summary>
        /////  添加默认数据库上下文
        ///// </summary>
        ///// <typeparam name="TDbContext">数据库上下文</typeparam>
        ///// <param name="services">服务</param>
        ///// <param name="optionBuilder">自定义配置</param>
        ///// <param name="interceptors">拦截器</param>
        ///// <returns>服务集合</returns>
        //public static IServiceCollection AddDb<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionBuilder, params IInterceptor[] interceptors)
        //    where TDbContext : DbContext
        //{
        //    // 避免重复注册默认数据库上下文
        //    if (DbContextHelper.DbContextDescriptors.ContainsKey(typeof(DefaultDbContextTypeProvider))) throw new InvalidOperationException("Prevent duplicate registration of default DbContext.");

        //    // 注册数据库上下文
        //    return services.AddDb<TDbContext, DefaultDbContextTypeProvider>(optionBuilder, interceptors);
        //}

        ///// <summary>
        ///// 添加数据库上下文
        ///// </summary>
        ///// <typeparam name="TDbContext">数据库上下文</typeparam>
        ///// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        ///// <param name="services">服务</param>
        ///// <param name="providerName">数据库提供器</param>
        ///// <param name="optionBuilder"></param>
        ///// <param name="connectionString">连接字符串</param>
        ///// <param name="interceptors">拦截器</param>
        ///// <returns>服务集合</returns>
        //public static IServiceCollection AddDb<TDbContext, TDbContextLocator>(this IServiceCollection services, string providerName = default, Action<DbContextOptionsBuilder> optionBuilder = null, string connectionString = default, params IInterceptor[] interceptors)
        //    where TDbContext : StarshineDbContext<TDbContext>
        //    where TDbContextLocator : class, IDbContextTypeProvider
        //{
        //    // 注册数据库上下文
        //    services.RegisterDbContext<TDbContext, TDbContextLocator>();

        //    // 配置数据库上下文
        //    var connStr = DbProvider.GetConnectionString<TDbContext>(connectionString);
        //    services.AddDbContext<TDbContext>(DbContextHelper.ConfigureDbContext(options =>
        //    {
        //        var _options = ConfigureDatabase<TDbContext>(providerName, connStr, options);
        //        optionBuilder?.Invoke(_options);
        //    }, interceptors));

        //    return services;
        //}

        ///// <summary>
        ///// 添加数据库上下文
        ///// </summary>
        ///// <typeparam name="TDbContext">数据库上下文</typeparam>
        ///// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        ///// <param name="services">服务</param>
        ///// <param name="optionBuilder">自定义配置</param>
        ///// <param name="interceptors">拦截器</param>
        ///// <returns>服务集合</returns>
        //public static IServiceCollection AddDb<TDbContext, TDbContextLocator>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionBuilder, params IInterceptor[] interceptors)
        //    where TDbContext : StarshineDbContext<TDbContext>
        //    where TDbContextLocator : class, IDbContextTypeProvider
        //{
        //    // 注册数据库上下文
        //    services.RegisterDbContext<TDbContext, TDbContextLocator>();

        //    // 配置数据库上下文
        //    services.AddDbContext<TDbContext>(DbContextHelper.ConfigureDbContext(optionBuilder, interceptors));

        //    return services;
        //}

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="provider">数据库提供器</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="options">数据库上下文选项构建器</param>
        private static DbContextOptionsBuilder ConfigureDatabase<TDbContext>(DatabaseProvider? provider, string? connectionString, DbContextOptionsBuilder options)
             where TDbContext : DbContext
        {
            var dbContextOptionsBuilder = options;

            // 获取数据库上下文特性
            var dbContextAttribute = DbProvider.GetAppDbContextAttribute(typeof(TDbContext));
            if (!string.IsNullOrWhiteSpace(connectionString) && provider != null)
            {
                provider ??= dbContextAttribute?.Provider;

                var useMethod = DbProviderHelper.GetDatabaseProviderUseMethod(provider!.Value);// 调用对应数据库程序集
                if (useMethod != null)
                {
                    // 处理最新第三方 MySql 包兼容问题
                    // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/commit/83c699f5b747253dc1b6fa9c470f469467d77686
                    if (provider == DatabaseProvider.MySql)
                    {
                        var mySqlVersion = DbProviderHelper.GetMySqlVersion(connectionString);
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object?[] { options, connectionString, mySqlVersion, MigrationsAssemblyAction }) as DbContextOptionsBuilder;
                    }
                    // 处理 Oracle 11 兼容问题
                    else if (provider == DatabaseProvider.Oracle)
                    {
                        Action<IRelationalDbContextOptionsBuilderInfrastructure> oracleOptionsAction = options =>
                        {
                            var optionsType = options.GetType();

                            // 处理版本号
                            optionsType.GetMethod("UseOracleSQLCompatibility")?
                                       .Invoke(options, new[] { version });

                            // 处理迁移程序集
                            optionsType.GetMethod("MigrationsAssembly")?
                                       .Invoke(options, new[] { DbContextHelper.DbSettings.MigrationAssemblyName });
                        };

                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object[] { options, connectionString, oracleOptionsAction }) as DbContextOptionsBuilder;
                    }
                    else
                    {
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object[] { options, connectionString, MigrationsAssemblyAction }) as DbContextOptionsBuilder;
                    }
                }

                
            }

            // 解决分表分库
            if (dbContextAttribute?.Mode == DbContextMode.Dynamic) dbContextOptionsBuilder
                  .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

            return dbContextOptionsBuilder;
        }

       
        /// <summary>
        /// 配置Code First 程序集 Action委托
        /// </summary>
        private static readonly Action<IRelationalDbContextOptionsBuilderInfrastructure> MigrationsAssemblyAction;

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static DatabaseProviderServiceCollectionExtensions()
        {
            DatabaseProviderUseMethodCollection = new ConcurrentDictionary<string, (MethodInfo, object)>();
            MigrationsAssemblyAction = options => options.GetType()
                .GetMethod("MigrationsAssembly")
                .Invoke(options, new[] { DbContextHelper.DbSettings.MigrationAssemblyName });
        }

       
        /// <summary>
        /// 解析数据库提供器信息
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        private static (string name, string version) ReadProviderInfo(string providerName)
        {
            // 解析真实的数据库提供器
            var providerNameAndVersion = providerName.Split('@', StringSplitOptions.RemoveEmptyEntries);
            providerName = providerNameAndVersion.First();

            string providerVersion = providerNameAndVersion.Length > 1 ? providerNameAndVersion[1] : default;
            return (providerName, providerVersion);
        }

        private static void CheckDbContextProvider<TDbContextProvider>()
            where TDbContextProvider : class, IDbContextTypeProvider
        { 
            
        }
    }
}