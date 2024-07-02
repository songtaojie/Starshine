using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 应用服务集合拓展类（由框架内部调用）
    /// </summary>
    [SkipScan]
    public static class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Web应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="config">配置文件</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddWebHostApp(this IServiceCollection services,IConfiguration config)
        {
            services.AddHostApp(s =>
            {
                // 添加 HttContext 访问器
                services.AddUserContext();

                // 注册swagger
                // 判断是否启用规范化文档
                if (StarshineApp.Settings.EnabledSwagger == true) services.AddSwaggerDocuments();

                // 判断是否启用规范化文档
                if (StarshineApp.Settings.EnabledUnifyResult == true) services.AddUnifyResult();

                //判断是否启用全局异常处理
                if (StarshineApp.Settings.EnabledExceptionFilter == true) services.AddFriendlyException();

                //判断是否启用全局异常处理
                if (StarshineApp.Settings.EnabledCors == true) services.AddCorsAccessor();
            });
            return services;
        }

        /// <summary>
        /// 添加主机应用配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configure">服务配置</param>
        /// <returns>服务集合</returns>
        internal static IServiceCollection AddHostApp(this IServiceCollection services, Action<IServiceCollection>? configure = default)
        {
            // 注册全局配置选项
            services.AddConfigureOptions<AppSettingsOptions>();

            // 注册全局依赖注入
            services.AddNativeDependencyInjection(StarshineApp.EffectiveTypes);

            // 自定义服务
            configure?.Invoke(services);

            return services;
        }

    }
}