// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Starshine.EntityFrameworkCore.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public class DbContextProvider: IDbContextProvider
{
    /// <summary>
    /// 数据库上下文 [StarshineDbContext] 特性缓存
    /// </summary>
    private readonly IConfiguration _configuration;
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public DbContextProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    /// <summary>
    /// 获取数据库上下文连接字符串
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <returns></returns>
    public string? GetConnectionString<TDbContext>()
        where TDbContext : DbContext
    {
        // 查找特性
        var dbContextAttribute = DbProviderHelper.GetStarshineDbContextAttribute(typeof(TDbContext));
        if (dbContextAttribute == null) return default;

        // 获取特性连接字符串
        var connStr = dbContextAttribute.ConnectionString;

        if (string.IsNullOrWhiteSpace(connStr)) return default;
        // 如果包含 = 符号，那么认为是连接字符串
        if (connStr.Contains("=")) return connStr;
        else
        {
            // 如果包含 : 符号，那么认为是一个 Key 路径
            if (connStr.Contains(":"))
            {
                return _configuration[connStr];
            }
            else
            {
                // 首先查找 DbConnectionString 键，如果没有找到，则当成 Key 去查找
                var connStrValue = _configuration.GetConnectionString(connStr);
                return string.IsNullOrWhiteSpace(connStrValue) ? _configuration[connStr] : connStrValue;
            }
        }
    }


    public MethodInfo GetDatabaseProviderUseMethod(DatabaseProvider databaseProvider, string version)
    { 
        
    }



}
