﻿using Hx.Swagger;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档中间件拓展
    /// </summary>
    public static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerConfigure"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocuments(this IApplicationBuilder app, Action<SwaggerOptions> swaggerConfigure = null, Action<SwaggerUIOptions> swaggerUIConfigure = null)
        {
            var builder = app.ApplicationServices.GetService<SwaggerDocumentBuilder>();
            // 配置 Swagger 全局参数
            app.UseSwagger(options => builder.Build(options, swaggerConfigure));
            // 配置 Swagger UI 参数
            app.UseSwaggerUI(options => builder.BuildUI(options, swaggerUIConfigure));
            return app;
        }

        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerConfigure"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerKnife4jDocuments(this IApplicationBuilder app, Action<SwaggerOptions> swaggerConfigure = null, Action<Knife4UIOptions> swaggerUIConfigure = null)
        {
            var builder = app.ApplicationServices.GetService<SwaggerDocumentBuilder>();
            // 配置 Swagger 全局参数
            app.UseSwagger(options => builder.Build(options, swaggerConfigure));

            // 配置 Swagger UI 参数
            app.UseKnife4UI(options => builder.BuildKnife4UI(options,swaggerUIConfigure));
            return app;
        }
    }
}