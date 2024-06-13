using Starshine.DatabaseAccessor;
using StackExchange.Profiling.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// MiniProfiler 服务拓展类
    /// </summary>
    public static class MiniProfilerServiceCollectionExtensions
    {
        /// <summary>
        /// MiniProfiler 插件路径
        /// </summary>
        private const string MiniProfilerRouteBasePath = "/index-mini-profiler";
        /// <summary>
        /// 添加MiniProfiler服务
        /// </summary>
        /// <param name="services">服务</param>
        /// <returns></returns>
        public static IServiceCollection AddMiniProfilerService(this IServiceCollection services)
        {
            // 注册MiniProfiler 组件
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = MiniProfilerRouteBasePath;
            }).AddRelationalDiagnosticListener();
            return services;
        }

        /// <summary>
        /// 添加 EF Core 进程监听拓展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMiniProfilerBuilder AddRelationalDiagnosticListener(this IMiniProfilerBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            builder.Services.AddSingleton<IMiniProfilerDiagnosticListener, RelationalDiagnosticListener>();

            return builder;
        }
    }
}
