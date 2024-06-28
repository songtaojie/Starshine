﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// StarshineDbContext配置
/// </summary>
public interface IStarshineDbContextOptionsBuilder
{
    /// <summary>
    /// 数据库提供商
    /// </summary>
    EfCoreDatabaseProvider? Provider { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    string? ConnectionString { get; set; }

    /// <summary>
    /// 迁移类库名称
    /// </summary>
    string? MigrationAssemblyName { get; set; }

    /// <summary>
    /// 数据库版本
    /// </summary>
    object? Version { get; set; }

    /// <summary>
    /// db配置
    /// </summary>
    DbContextOptionsBuilder DbContextOptions { get;}

    /// <summary>
    /// 添加<see cref="IInterceptor" />实例到那些在上下文中注册的实例。
    /// </summary>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    DbContextOptionsBuilder AddInterceptors(params IInterceptor[] interceptors);

    /// <summary>
    /// 添加<see cref="IInterceptor" />实例到那些在上下文中注册的实例。
    /// </summary>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    DbContextOptionsBuilder AddInterceptors(IEnumerable<IInterceptor> interceptors);
}
