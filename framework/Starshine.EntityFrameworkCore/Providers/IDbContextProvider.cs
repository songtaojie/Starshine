// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// DbContext
/// </summary>
public interface IDbContextProvider
{
    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <returns></returns>
    string? GetConnectionString<TDbContext>()
        where TDbContext : DbContext;
}
