using Starshine.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    public static class SwaggerDocumentServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="swaggerSettings">swagger配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStarshineSwagger(this IServiceCollection services, Action<SwaggerSettingsOptions>? swaggerSettings = default)
        {
            services.AddOptions<SwaggerSettingsOptions>()
                .BindConfiguration("SwaggerSettings")
                .Configure(options =>
                {
                    options.Configure(options);
                    swaggerSettings?.Invoke(options);
                });
            services.AddSingleton<SwaggerDocumentBuilder>();
            services.AddSwaggerGen();
            services.AddOptions<SwaggerGenOptions>()
                .Configure<SwaggerDocumentBuilder>((options, builder) =>
                {
                    builder.BuildSwaggerGen(options);
                });

            return services;
        }

        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="mvcBuilder">Mvc 构建器</param>
        /// <param name="swaggerSettings">swagger配置</param>
        /// <returns>服务集合</returns>
        public static IMvcBuilder AddStarshineSwagger(this IMvcBuilder mvcBuilder, Action<SwaggerSettingsOptions>? swaggerSettings = default)
        {
            mvcBuilder.Services.AddStarshineSwagger(swaggerSettings);
            return mvcBuilder;
        }
    }
}