// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using Starshine.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// StarshineEfCoreBuilder扩展
/// </summary>
public static class StarshineEfCoreBuilderExtensions
{
    /// <summary>
    /// MiniProfiler 插件路径
    /// </summary>
    private const string MiniProfilerRouteBasePath = "/index-mini-profiler";

    /// <summary>
    /// 添加MiniProfiler服务,实现 EF Core 进程监听拓展
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IStarshineEfCoreBuilder AddMiniProfilerService(this IStarshineEfCoreBuilder builder, Action<MiniProfilerOptions>? configureOptions = null)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        // 注册MiniProfiler 组件
        builder.Services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = MiniProfilerRouteBasePath;
            configureOptions?.Invoke(options);
        });

        builder.Services.AddSingleton<IMiniProfilerDiagnosticListener, RelationalDiagnosticListener>();
        return builder;
    }
}
