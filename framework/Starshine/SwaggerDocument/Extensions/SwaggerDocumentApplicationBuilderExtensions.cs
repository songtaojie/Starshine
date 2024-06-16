using Starshine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档swagger中间件拓展
    /// </summary>
    [SkipScan]
    internal static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder app)
        {
            // 判断是否安装了 DependencyInjection 程序集
            var logger = app.ApplicationServices.GetService<ILogger<HxCoreApp>>();
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return app;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
            if (swaggerBuilderExtensionsType == null) return app;
            var useSwaggerDocuments = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "UseSwaggerDocuments" && u.GetParameters().First()?.ParameterType == typeof(IApplicationBuilder));
            logger.LogDebug("Use the SwaggerUI ApplicationBuilder");
            useSwaggerDocuments?.Invoke(null, new object[] { app, null, null });
            return app;

        }

        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseSwaggerKnife4jDocuments(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<HxCoreApp>>();
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return app;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
            if (swaggerBuilderExtensionsType == null) return app;
            var useSwaggerDocuments = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "UseSwaggerKnife4jDocuments" && u.GetParameters().First().ParameterType == typeof(IApplicationBuilder));
            logger.LogDebug("Use the Swagger Knife4UI ApplicationBuilder");
            useSwaggerDocuments?.Invoke(null, new object[] { app, null, null});
            return app;
        }
    }
}