using Hx.Sdk.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 状态码中间件拓展
    /// </summary>
    [SkipScan]
    public static class UnifyResultMiddlewareExtensions
    {
        /// <summary>
        /// 添加状态码拦截中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnifyResultStatusCodes(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetService<ILogger<HxCoreApp>>();
            // 提供配置
            app.UseMiddleware<UnifyResultStatusCodesMiddleware>();
            logger.LogDebug("Use the UnifyResultStatusCodes ApplicationBuilder");
            return app;
        }
    }
}