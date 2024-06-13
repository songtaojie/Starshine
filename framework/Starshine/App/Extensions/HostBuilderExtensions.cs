using Starshine.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 主机构建器拓展类
    /// </summary>
    [SkipScan]
    public static class HostBuilderExtensions
    {

        /// <summary>
        /// 用预先配置的默认值初始化类的新实例<see cref="IWebHostBuilder"/>
        /// 注意，使用此扩展方法后代替ConfigureWebHostDefaults会自动配置IWebHostBuilder的ConfigureHxWebApp
        /// </summary>
        /// <remarks>
        ///    以下默认值应用于<see cref="IWebHostBuilder"/>:
        ///    使用Kestrel作为web服务器并使用应用程序的配置提供商来配置它，
        ///    添加HostFiltering中间件，
        ///    如果ASPNETCORE_FORWARDEDHEADERS_ENABLED=true则添加ForwardedHeaders中间件，
        ///    并启用IIS集成。
        /// </remarks> 
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="configure">配置回调</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxWebHostDefaults(this IHostBuilder hostBuilder, Action<IWebHostBuilder> configure = null)
        {
            // 获取命令行参数
            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.ConfigureHxWebApp();
                configure(webHostBuilder);
            });
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机配置
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxApp(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHxAppConfiguration();
            return hostBuilder;
        }

        /// <summary>
        /// 泛型主机配置Configuration
        /// </summary>
        /// <param name="hostBuilder">泛型主机注入构建器</param>
        /// <param name="configureDelegate">配置对象</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder ConfigureHxAppConfiguration(this IHostBuilder hostBuilder, Action<HostBuilderContext, IConfigurationBuilder> configureDelegate = null)
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                // 存储环境对象
                InternalApp.HostEnvironment = hostingContext.HostingEnvironment;

                // 加载配置
                InternalApp.AddConfigureFiles(config, InternalApp.HostEnvironment);
                configureDelegate?.Invoke(hostingContext, config);
            });
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                // 存储服务提供器
                InternalApp.InternalServices = services;
                // 存储配置对象
                InternalApp.Configuration = hostContext.Configuration;
                // 存储服务提供器
                services.AddHostedService<GenericHostLifetimeEventsHostedService>();
                // 初始化应用服务
                services.AddHostApp();
            });
            return hostBuilder;
        }
    }
}