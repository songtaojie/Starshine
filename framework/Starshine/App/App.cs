using Starshine.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Starshine.Internal;

namespace Starshine
{
    /// <summary>
    /// 全局应用类
    /// </summary>
    [SkipScan]
    public static partial class App
    {

        /// <summary>
        /// 应用有效程序集
        /// </summary>
        public static readonly IEnumerable<Assembly> Assemblies;

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        public static readonly IEnumerable<Type> EffectiveTypes;

        /// <summary>
        /// 私有设置，避免重复解析
        /// </summary>
        private static AppSettingsOptions? _settings;

        /// <summary>
        /// 应用全局配置
        /// </summary>
        internal static AppSettingsOptions Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = GetConfig<AppSettingsOptions>("AppSettings") ?? new AppSettingsOptions();
                    _settings.PostConfigure(nameof(AppSettingsOptions), _settings);
                }
                return _settings;
            }
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService? GetService<TService>(IServiceProvider? scoped = default)
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
        public static object? GetService(Type type, IServiceProvider? scoped = default)
        {
            return (scoped ?? InternalApp.RootServices!).GetService(type);
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static TService GetRequiredService<TService>(IServiceProvider? scoped = default)
            where TService : class
        {
            return (scoped ?? InternalApp.RootServices!).GetRequiredService<TService>();
        }

        /// <summary>
        /// 获取请求生命周期的服务
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static object GetRequiredService(Type type, IServiceProvider? scoped = default)
        {
            return (scoped ?? InternalApp.RootServices!).GetRequiredService(type);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        static App()
        {
            Assemblies = GetAssemblies();
            EffectiveTypes = Assemblies.SelectMany(u => u.GetTypes()
                .Where(u => u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false)));
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

            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Hx.Sdk 发布的包，或手动添加引用的dll，或配置特定的包前缀
            var packages = dependencyContext.CompileLibraries
                .Where(u =>
                       (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j)))
                       || (u.Type == "package" && u.Name.StartsWith("Hx")));

            var scanAssemblies = packages.Select(u => AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(u.Name)))
            .ToList();

            return scanAssemblies;
        }

        #region 服务配置
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name">ConnectionStrings节点中子节点名字</param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return (InternalApp.Configuration ?? GetService<IConfiguration>()).GetConnectionString(name);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="jsonKey">配置中对应的Key</param>
        /// <returns>TOptions</returns>
        public static TOptions? GetConfig<TOptions>(string jsonKey)
        {
            var config = InternalApp.Configuration ?? GetService<IConfiguration>();
            if (config == null) return default;
            return config.GetSection(jsonKey).Get<TOptions>();
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions? GetOptions<TOptions>(IServiceProvider? scoped = default)
            where TOptions : class, new()
        {
            return GetService<IOptions<TOptions>>(scoped)?.Value;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions? GetOptionsMonitor<TOptions>(IServiceProvider? scoped = default)
            where TOptions : class, new()
        {
            return GetService<IOptionsMonitor<TOptions>>(scoped)?.CurrentValue;
        }

        /// <summary>
        /// 获取选项
        /// </summary>
        /// <typeparam name="TOptions">强类型选项类</typeparam>
        /// <param name="scoped"></param>
        /// <returns>TOptions</returns>
        public static TOptions? GetOptionsSnapshot<TOptions>(IServiceProvider? scoped = default)
            where TOptions : class, new()
        {
            return GetService<IOptionsSnapshot<TOptions>>(scoped)?.Value;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">配置节点</param>
        /// <returns></returns>
        public static string GetConfig(params string[] sections)
        {
            try
            {
                if (sections == null || sections.Length == 0) throw new ArgumentNullException(nameof(sections));
                var config = InternalApp.Configuration ?? GetService<IConfiguration>();
                if (config == null) return string.Empty;
                if (sections.Length == 1)
                {
                    return config[sections[0]];
                }
                else
                {
                    string section = string.Join(':', sections);
                    return config[section];
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
