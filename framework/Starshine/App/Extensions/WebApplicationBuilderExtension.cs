using Starshine.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// WebApplication构建器扩展
    /// </summary>
    public static class WebApplicationBuilderExtension
    {
        /// <summary>
        /// Web 应用注入
        /// </summary>
        /// <param name="webApplicationBuilder">Web应用构建器</param>
        /// <returns>WebApplicationBuilder</returns>
        public static WebApplicationBuilder ConfigureHxWebApp(this WebApplicationBuilder webApplicationBuilder)
        {
            //InternalApp.WebHostEnvironment = webApplicationBuilder.Environment;
            // 初始化配置
            webApplicationBuilder.WebHost.ConfigureHxWebAppConfiguration();
            return webApplicationBuilder;
        }
    }
}
