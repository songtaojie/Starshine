// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public class DbContextCreationContext
{
    /// <summary>
    /// 当前DbContextCreationContext对象
    /// </summary>
    public static DbContextCreationContext Current => _current.Value!;
    private static readonly AsyncLocal<DbContextCreationContext> _current = new AsyncLocal<DbContextCreationContext>();

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// 存在的连接
    /// </summary>
    public DbConnection? ExistingConnection { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString"></param>
    public DbContextCreationContext(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IDisposable Use(DbContextCreationContext context)
    {
        var previousValue = Current;
        _current.Value = context;
        return new DisposeAction(() =>
        {
            _current.Value = previousValue;
        });
    }
}
