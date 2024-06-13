using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 应用全局配置
    /// </summary>
    public sealed class AppSettingsOptions : IPostConfigureOptions<AppSettingsOptions>
    {
        /// <summary>
        /// 是否启用规范化文档Swagger
        /// </summary>
        public bool? EnabledSwagger { get; set; }

        /// <summary>
        /// swagger配置
        /// </summary>
        public SwaggerUIEnum? SwaggerUI { get; set; }

        /// <summary>
        /// 是否打印数据库连接信息到 MiniProfiler 中
        /// </summary>
        public bool? PrintDbConnectionInfo { get; set; }

        /// <summary>
        /// 是否使用IdentityServer4授权认证，false：使用jwt授权认证
        /// </summary>
        public bool? UseIdentityServer4 { get; set; }

        /// <summary>
        /// 是否启用规范化结果
        /// </summary>
        public bool? EnabledUnifyResult { get; set; }

        /// <summary>
        /// 是否启用事件总线
        /// </summary>
        public bool? EnabledCap { get; set; }

        /// <summary>
        /// 是否开启全局异常过滤器
        /// </summary>
        public bool? EnabledExceptionFilter { get; set; }

        /// <summary>
        /// 是否开启sql日志记录
        /// </summary>
        public bool? EnabledSqlLog { get; set; }

        /// <summary>
        /// 是否开启跨域
        /// </summary>
        public bool? EnabledCors { get; set; }
        
        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string name,AppSettingsOptions options)
        {
            options.EnabledSwagger ??= false;
            options.SwaggerUI ??= SwaggerUIEnum.Original;
            options.PrintDbConnectionInfo ??= true;
            options.UseIdentityServer4 ??= false;
            options.EnabledUnifyResult ??= true;
            options.EnabledExceptionFilter ??= true;
            options.EnabledSqlLog ??= false;
            options.EnabledCap ??= false;
            options.EnabledCors ??= true;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public enum SwaggerUIEnum
    { 
        /// <summary>
        /// 原生ui的swagger
        /// </summary>
        Original = 1,
        /// <summary>
        /// 使用Knife4UI的swagger
        /// </summary>
        Knife4 = 2
    }
}
