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
        internal static IApplicationBuilder UseStarshineSwagger(this IApplicationBuilder app)
        {
            // 判断是否安装了 DependencyInjection 程序集
            
            var diAssembly = StarshineApp.Assemblies.FirstOrDefault(u => u.GetName().Name!.Equals(AppExtend.Swagger));
            if (diAssembly == null) return app;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
            if (swaggerBuilderExtensionsType == null) return app;
            var useStarshineSwagger = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(u => u.Name == "UseStarshineSwagger" && u.GetParameters().First()?.ParameterType == typeof(IApplicationBuilder));
            if(useStarshineSwagger == null)return app;
            var logger = app.ApplicationServices.GetRequiredService<ILogger<IApplicationBuilder>>();
            logger.LogDebug("Use the SwaggerUI ApplicationBuilder");
            useStarshineSwagger?.Invoke(null, new object?[] { app, null, null });
            return app;

        }

        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseStarshineSwaggerKnife4j(this IApplicationBuilder app)
        {
            
            // 判断是否安装了 DependencyInjection 程序集
            var diAssembly = StarshineApp.Assemblies.FirstOrDefault(u => u.GetName().Name!.Equals(AppExtend.Swagger));
            if (diAssembly == null) return app;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.AspNetCore.Builder.SwaggerDocumentApplicationBuilderExtensions");
            if (swaggerBuilderExtensionsType == null) return app;
            var useStarshineSwagger = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(u => u.Name == "UseStarshineSwaggerKnife4j" && u.GetParameters().First().ParameterType == typeof(IApplicationBuilder));
            if(useStarshineSwagger == null) return app;
            var logger = app.ApplicationServices.GetRequiredService<ILogger<IApplicationBuilder>>();
            logger.LogDebug("Use the Swagger Knife4UI ApplicationBuilder");
            useStarshineSwagger?.Invoke(null, new object?[] { app, null, null});
            return app;
        }
    }
}