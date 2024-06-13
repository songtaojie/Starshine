using Starshine.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Starshine
{
    /// <summary>
    /// 应用启动时自动注册中间件
    /// </summary>
    /// <remarks>
    /// </remarks>
    [SkipScan]
    public class HxAppStartupFilter : IStartupFilter
    {
        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                InternalApp.RootServices = app.ApplicationServices;
                var hostEnvironment = app.ApplicationServices.GetService<IHostEnvironment>();
                // 设置响应报文头信息，标记框架类型
                // 环境名
                var envName = hostEnvironment?.EnvironmentName ?? "Unknown";

                // 设置响应报文头信息
                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering();  // 启动 Request Body 重复读，解决微信问题
                    context.Response.Headers["environment"] = envName;
                    await next.Invoke();
                });
                // 调用默认中间件
                app.UseHxApp();
                next(app);
            };
        }
    }
}
