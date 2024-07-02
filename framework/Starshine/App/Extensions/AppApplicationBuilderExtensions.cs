
namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 应用中间件拓展类
    /// </summary>
    [SkipScan]
    public static class AppApplicationBuilderExtensions
    {

        /// <summary>
        /// 添加应用中间件
        /// </summary>
        /// <param name="app">应用构建器</param>
        /// <param name="configure">应用配置</param>
        /// <returns>应用构建器</returns>
        internal static IApplicationBuilder UseStarshineApp(this IApplicationBuilder app, Action<IApplicationBuilder>? configure = default)
        {

            // 判断是否启用规范化文档
            if (StarshineApp.Settings.EnabledSwagger == true)
            {
                if (StarshineApp.Settings.SwaggerUI == SwaggerUIEnum.Knife4)
                {
                    app.UseSwaggerKnife4jDocuments();
                }
                else
                {
                    app.UseSwaggerDocuments();
                }
            }

            if (StarshineApp.Settings.EnabledUnifyResult == true) app.UseUnifyResultStatusCodes();

            if (StarshineApp.Settings.EnabledCors == true) app.UseCorsAccessor();
            // 调用自定义服务
            configure?.Invoke(app);
            return app;
        }
    }
}