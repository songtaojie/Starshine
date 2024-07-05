// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 连接字符串解析器
/// </summary>
public interface IConnectionStringResolver
{
    /// <summary>
    /// 解析数据库连接字符串
    /// </summary>
    /// <returns></returns>
    Task<string?> ResolveAsync<TDbContext>() where TDbContext : DbContext;
}
