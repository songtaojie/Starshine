// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    [SkipScan]
    internal static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddStarshineSwagger(this IServiceCollection services)
        {
            // 判断是否安装了 Starshine.Swagger 程序集
            var diAssembly = StarshineApp.Assemblies.FirstOrDefault(u => u.GetName().Name!.Equals(AppExtend.Swagger));
            if (diAssembly == null) return services;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
            if (swaggerBuilderExtensionsType == null) return services;
            var addStarshineSwaggerMethod = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(AddStarshineSwaggerMethod);
            var logger = NullLoggerFactory.Instance.CreateLogger<IServiceCollection>();
            if (addStarshineSwaggerMethod == null)
            {
                logger.LogDebug($"No method AddStarshineSwagger was found in the assembly {diAssembly.FullName}");
                return services;
            }
            logger.LogDebug("The AddStarshineSwagger method is initialized");
            addStarshineSwaggerMethod?.Invoke(null, new object?[] { services, null});
            return services;
            static bool AddStarshineSwaggerMethod(MethodInfo methodInfo)
            {
                var parameter = methodInfo.GetParameters();
                return methodInfo.Name == "AddStarshineSwagger" && parameter.Length == 2
                    && parameter.First().ParameterType == typeof(IServiceCollection);
            }
        }
    }
}
