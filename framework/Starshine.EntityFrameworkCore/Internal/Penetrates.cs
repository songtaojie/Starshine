using Starshine.EntityFrameworkCore.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class Penetrates
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection InternalServices;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// 服务提供器
        /// </summary>
        internal static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null) _serviceProvider = InternalServices.BuildServiceProvider();
                return _serviceProvider;
            }
            set
            {
                _serviceProvider = value;
            }
        }

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        private static DbSettingsOptions _dbsettings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        internal static DbSettingsOptions DbSettings
        {
            get
            {
                if (_dbsettings == null)
                {
                    var options = GetService<IOptions<DbSettingsOptions>>();
                    _dbsettings = options == null ? new DbSettingsOptions() : options.Value;
                }
                return _dbsettings;
            }
        }

        /// <summary>
        /// 数据库上下文描述器
        /// </summary>
        internal static readonly ConcurrentDictionary<Type, Type> DbContextDescriptors;
        /// <summary>
        /// 应用有效程序集
        /// </summary>
        internal static readonly IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        internal static readonly IEnumerable<Type> EffectiveTypes;
        /// <summary>
        /// 构造函数
        /// </summary>
        static Penetrates()
        {
            DbContextDescriptors = new ConcurrentDictionary<Type, Type>();

            Assemblies = GetAssemblies();

            EffectiveTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic));
        }

        /// <summary>
        /// 配置 SqlServer 数据库上下文
        /// </summary>
        /// <param name="optionBuilder">数据库上下文选项构建器</param>
        /// <param name="interceptors">拦截器</param>
        /// <returns></returns>
        internal static Action<IServiceProvider, DbContextOptionsBuilder> ConfigureDbContext(Action<DbContextOptionsBuilder> optionBuilder, params IInterceptor[] interceptors)
        {
            return (scoped, options) =>
            {

                if (DbSettings.EnabledSqlLog == true)
                {
                    ILoggerFactory loggerFactory = scoped.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
                    options.UseLoggerFactory(loggerFactory)
                                .EnableDetailedErrors()
                                .EnableSensitiveDataLogging();
                }

                optionBuilder.Invoke(options);
                // 添加拦截器
                AddInterceptors(interceptors, options);

                // .NET 5 版本已不再起作用
                //options.UseInternalServiceProvider(scoped);
            };
        }

        /// <summary>
        /// 数据库数据库拦截器
        /// </summary>
        /// <param name="interceptors">拦截器</param>
        /// <param name="options"></param>
        private static void AddInterceptors(IInterceptor[] interceptors, DbContextOptionsBuilder options)
        {
            // 添加拦截器
            var interceptorList = DbProvider.GetDefaultInterceptors();

            if (DbSettings.EnabledMiniProfiler == true)
            {
                interceptorList.Add(new SqlConnectionProfilerInterceptor());
            }
            if (interceptors != null || interceptors.Length > 0)
            {
                interceptorList.AddRange(interceptors);
            }
            options.AddInterceptors(interceptorList.ToArray());
        }

        /// <summary>
        /// 检查数据库上下文是否绑定
        /// </summary>
        /// <param name="dbContextLocatorType"></param>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        internal static void CheckDbContextLocator(Type dbContextLocatorType, out Type dbContextType)
        {
            if (!DbContextDescriptors.TryGetValue(dbContextLocatorType, out dbContextType)) throw new InvalidCastException($" The dbcontext locator `{dbContextLocatorType.Name}` is not bind.");
        }


        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        internal static TService GetService<TService>(IServiceProvider scoped = default)
            where TService : class
        {
            return GetService(typeof(TService), scoped) as TService;
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        internal static object GetService(Type type, IServiceProvider scoped = default)
        {
            return (scoped ?? ServiceProvider).GetService(type);
        }

        /// <summary>
        /// 打印验证信息到 MiniProfiler
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="isError">是否为警告消息</param>
        public static void PrintToMiniProfiler(string category, string state, string message = null, bool isError = false)
        {
            // 判断是否启用了 MiniProfiler 组件
            if (DbSettings.EnabledMiniProfiler != true) return;

            // 打印消息
            string titleCaseategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
            var customTiming = MiniProfiler.Current.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseategory} {state}" : message, state);
            if (customTiming == null) return;

            // 判断是否是警告消息
            if (isError) customTiming.Errored = true;
        }

        /// <summary>
        /// 获取应用有效程序集
        /// </summary>
        /// <returns>IEnumerable</returns>
        private static IEnumerable<Assembly> GetAssemblies()
        {
            // 需排除的程序集后缀
            var excludeAssemblyNames = new string[] {
                "Database.Migrations"
            };

            // 读取应用配置
            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
            var scanAssemblies = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j)))
                       || (u.Type == "package" && u.Name.StartsWith("Hx.Sdk")))
                .Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
                .ToList();

            return scanAssemblies;
        }
    }
}
