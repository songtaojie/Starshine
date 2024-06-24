// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 上下文配置
/// </summary>
public class StarshineDbContextOptions
{
    /// <summary>
    /// 数据库提供商
    /// </summary>
    public DatabaseProvider?  Provider { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string? ConnectionString {  get; set; }  

    /// <summary>
    /// 数据库版本
    /// </summary>
    public object? Version {  get; set; }

    /// <summary>
    /// db
    /// </summary>
    internal Action<DbContextOptionsBuilder>? OptionsBuilder { get; private set; }

    /// <summary>
    /// 连接池大小
    /// </summary>
    public int PoolSize { get; set; } = 100;

    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="optionsBuilder"></param>
    public void Configure(Action<DbContextOptionsBuilder> optionsBuilder)
    {
        OptionsBuilder = optionsBuilder;
    }
}
