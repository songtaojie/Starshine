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

    }
}
