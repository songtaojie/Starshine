// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 数据库提供商
/// </summary>
public enum EfCoreDatabaseProvider
{
    /// <summary>
    /// 内存数据库
    /// </summary>
    InMemory,
    /// <summary>
    /// Sqlite数据库
    /// </summary>
    Sqlite,
    /// <summary>
    /// SqlServer 数据库
    /// </summary>
    SqlServer,
    /// <summary>
    /// MySql数据库,使用Pomelo.EntityFrameworkCore.MySql
    /// </summary>
    MySql,
    /// <summary>
    /// MySql数据库,使用MySql.EntityFrameworkCore
    /// </summary>
    MySqlOfficial,
    /// <summary>
    /// Oracle数据库
    /// </summary>
    Oracle,
    /// <summary>
    /// PostgreSQL数据库
    /// </summary>
    PostgreSQL,
    /// <summary>
    /// Firebird数据库
    /// </summary>
    Firebird,
    /// <summary>
    /// Cosmos 数据库
    /// </summary>
    Cosmos,
    /// <summary>
    /// Dm数据库
    /// </summary>
    Dm
}
