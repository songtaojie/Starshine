using Hx.Sdk.Core.CorsAccessor;
using Hx.Sdk.Core.CorsAccessor.Internal;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 跨域访问服务拓展类
    /// </summary>
    [SkipScan]
    public static class CorsAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="corsOptionsHandler"></param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCorsAccessor(this IServiceCollection services, Action<CorsOptions> corsOptionsHandler = default)
        {
            // 添加跨域配置选项
            services.AddConfigureOptions<CorsAccessorSettingsOptions>();
            services.AddCors();
            services.AddOptions<CorsOptions>()
                .Configure<IOptions<CorsAccessorSettingsOptions>>((corsOptions, corsAccessorSettingsOptions) =>
                {
                    var corsAccessorSettings = corsAccessorSettingsOptions.Value;
                    // 添加策略跨域
                    corsOptions.AddPolicy(name: corsAccessorSettings.PolicyName, builder =>
                    {
                        CorsAccessorPolicy.SetCorsPolicy(builder, corsAccessorSettings);
                    });
                    // 添加自定义配置
                    corsOptionsHandler?.Invoke(corsOptions);
                });
            return services;
        }
    }
}