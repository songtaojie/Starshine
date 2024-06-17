using Starshine.Common;
using Starshine.Common.Extensions;
using Starshine.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Web 主机配置
        /// </summary>
        /// <param name="webHostBuilder">Web主机构建器</param>
        /// <returns>IWebHostBuilder</returns>
        internal static IWebHostBuilder ConfigureHxWebApp(this IWebHostBuilder webHostBuilder)
        {
            // 获取默认程序集名称
            var defaultAssemblyName = typeof(HostBuilderExtensions).GetAssemblyName();

            //  获取环境变量 ASPNETCORE_HOSTINGSTARTUPASSEMBLIES 配置
            var environmentVariables = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES");
            var combineAssembliesName = $"{defaultAssemblyName};{environmentVariables}".Trim(';');

            webHostBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, combineAssembliesName);
            return webHostBuilder;
        }

        /// <summary>
        /// Web主机配置Configuration
        /// </summary>
        /// <param name="webHostBuilder">泛型主机注入构建器</param>
        /// <param name="configureDelegate">配置对象</param>
        /// <returns>IHostBuilder</returns>
        public static IWebHostBuilder ConfigureHxWebAppConfiguration(this IWebHostBuilder webHostBuilder, Action<WebHostBuilderContext, IConfigurationBuilder>? configureDelegate = default)
        {
            // 自动装载配置
            webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                InternalApp.SetWebHostEnvironment(hostingContext.HostingEnvironment);
                InternalApp.SetHostEnvironment(hostingContext.HostingEnvironment);

                // 加载配置
                InternalApp.AddConfigureFiles(config, hostingContext.HostingEnvironment);
                configureDelegate?.Invoke(hostingContext, config);
            });

            webHostBuilder.ConfigureServices((hostContext, services) =>
            {
                var config = hostContext.Configuration;
                // 存储服务提供器
                InternalApp.SetServiceCollection(services);
                // 存储配置对象
                InternalApp.SetConfiguration(config);
                // 存储服务提供器
                services.AddHostedService<GenericHostLifetimeEventsHostedService>();
                // 注册 Startup 过滤器
                services.AddTransient<IStartupFilter, HxAppStartupFilter>();
                // 初始化应用服务
                services.AddWebHostApp(config);
            });
            return webHostBuilder;
        }
    }
}
