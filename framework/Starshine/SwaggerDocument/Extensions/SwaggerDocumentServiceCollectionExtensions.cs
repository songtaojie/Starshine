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
        internal static IServiceCollection AddSwaggerDocuments(this IServiceCollection services)
        {
            // 判断是否安装了 Starshine.Swagger 程序集
            var diAssembly = App.Assemblies.FirstOrDefault(u => u.GetName().Name.Equals(AppExtend.Swagger));
            if (diAssembly == null) return services;
            // 加载 SwaggerBuilder 拓展类型和拓展方法
            var swaggerBuilderExtensionsType = diAssembly.GetType($"Microsoft.Extensions.DependencyInjection.SwaggerDocumentServiceCollectionExtensions");
            if (swaggerBuilderExtensionsType == null) return services;
            var addSwaggerDocumentsMethod = swaggerBuilderExtensionsType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(FirstMethod);
            var logger = NullLoggerFactory.Instance.CreateLogger<HxCoreApp>();
            logger.LogDebug("Add the Swagger service");
            addSwaggerDocumentsMethod?.Invoke(null, new object[] { services, null, null });
            return services;
            static bool FirstMethod(MethodInfo methodInfo)
            {
                var parameter = methodInfo.GetParameters();
                if (parameter.Length < 2) return false;
                return methodInfo.Name == "AddSwaggerDocuments"
                    && parameter.First().ParameterType == typeof(IServiceCollection);
            }
        }
    }
}
