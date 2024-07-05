// MIT License
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
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// StarshineDbContext配置
/// </summary>
public interface IStarshineDbContextOptionsBuilder
{
    /// <summary>
    /// 服务
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// 数据库提供商
    /// </summary>
    EFCoreDatabaseProvider? Provider { get; set; }

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
    /// 
    /// </summary>
    /// <param name="dbContextOptions"></param>
    void Configure([NotNull] Action<DbContextOptionsBuilder> dbContextOptions);

    /// <summary>
    /// 为这个DbContext中的所有实体注册默认存储库。
    /// </summary>
    /// <returns></returns>
    IStarshineDbContextOptionsBuilder AddDefaultRepositories();
}
