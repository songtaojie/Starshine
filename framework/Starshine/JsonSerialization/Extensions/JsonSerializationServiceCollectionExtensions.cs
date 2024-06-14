﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc;
using Starshine.JsonSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// Json 序列化服务拓展类
/// </summary>
public static class JsonSerializationServiceCollectionExtensions
{
    /// <summary>
    /// 配置 Json 序列化提供器
    /// </summary>
    /// <typeparam name="TJsonSerializerProvider"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsonSerialization<TJsonSerializerProvider>(this IServiceCollection services)
        where TJsonSerializerProvider : class, IJsonSerializerProvider
    {
        services.AddSingleton<IJsonSerializerProvider, TJsonSerializerProvider>();
        return services;
    }

    /// <summary>
    /// 配置 JsonOptions 序列化选项
    /// <para>主要给非 Web 环境使用</para>
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsonOptions(this IServiceCollection services, Action<JsonOptions> configure)
    {
        // 手动添加配置
        services.Configure<JsonOptions>(options =>
        {
            configure?.Invoke(options);
        });

        return services;
    }
}
