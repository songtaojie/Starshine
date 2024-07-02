// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using Starshine.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// 添加数据库上下文
    /// </summary>
    /// <param name="builder">StarshineEfCore构建器</param>
    /// <returns>服务集合</returns>
    public static IStarshineEfCoreBuilder AddStarshineRepositories(this IStarshineEfCoreBuilder builder)
    {
        // 注册数据库上下文池
        AddStarshineRepositories(builder.Services);
        return builder;
    }


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

    private static IServiceCollection AddStarshineRepositories(IServiceCollection services)
    {

        // 注册多数据库上下文仓储
        services.TryAddScoped(typeof(IEfCoreRepository<>), typeof(EFCoreRepository<,>));

        // 注册泛型仓储
        services.TryAddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        // 注册非泛型仓储
        services.TryAddScoped(typeof(IRepository), typeof(EFCoreRepository<,>));
      
        return services;
    }


}
