using Starshine;
using Starshine.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Web相关的拓展类
    /// </summary>
    public static class WebServiceCollectionExtensions
    {
        /// <summary>
        /// 添加UserContent,可以获取用户信息
        /// 操作cookie
        /// 使用时只需要在构造函数注入IUserContext即可
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddUserContext(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddHttpContextAccessor();
            services.AddScoped<UserManager>();
            return services;
        }

        /// <summary>
        /// 添加web管理类用于一些路径的处理
        /// 使用时只需要在构造函数注入IWebManager即可
        /// </summary>
        /// <param name="services">服务</param>
        public static IServiceCollection AddWebManager(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<WebManager>();
            return services;
        }



        /// <summary>
        /// 添加HttpClient，进行远程请求
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddHxHttpClient(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var exist = services.Any(x => x.ServiceType == typeof(IHxHttpClient));
            if (exist) return services;
            services.AddHttpClient<IHxHttpClient, HxHttpClient>()
                // 忽略 SSL 不安全检查，或 https 不安全或 https 证书有误
                .ConfigurePrimaryHttpMessageHandler(u => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                })
                // 设置客户端生存期为 5 分钟
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            return services;
        }
    }
}
