// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
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
    /// 
    /// </summary>
    /// <param name="dbContextOptions"></param>
    public StarshineDbContextOptions(DbContextOptionsBuilder dbContextOptions)
    {
        DbContextOptions = dbContextOptions;
    }
    /// <summary>
    /// 数据库提供商
    /// </summary>
    public EfCoreDatabaseProvider?  Provider { get; set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string? ConnectionString {  get; set; }

    /// <summary>
    /// 迁移类库名称
    /// </summary>
    public string? MigrationAssemblyName { get; set; }

    /// <summary>
    /// 数据库版本
    /// </summary>
    public object? Version {  get; set; }

    /// <summary>
    /// db配置
    /// </summary>
    public DbContextOptionsBuilder DbContextOptions { get; private set; }

    /// <summary>
    /// 添加<see cref="IInterceptor" />实例到那些在上下文中注册的实例。
    /// </summary>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    public virtual DbContextOptionsBuilder AddInterceptors(params IInterceptor[] interceptors)
           => DbContextOptions.AddInterceptors(interceptors);

    /// <summary>
    /// 添加<see cref="IInterceptor" />实例到那些在上下文中注册的实例。
    /// </summary>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    public virtual DbContextOptionsBuilder AddInterceptors(IEnumerable<IInterceptor> interceptors)
            => DbContextOptions.AddInterceptors(interceptors);

    /// <summary>
    /// 获取hashcode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        if (Provider != null) hashCode.Add(Provider);
        if(!string.IsNullOrEmpty(ConnectionString)) hashCode.Add(ConnectionString);
        if(Version!=null) hashCode.Add(Version);
        hashCode.Add(this.DbContextOptions.Options.GetHashCode());
        return hashCode.ToHashCode();
    }
}
