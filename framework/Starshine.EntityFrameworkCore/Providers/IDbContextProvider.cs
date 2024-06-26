﻿// MIT License
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
/// DbContext提供器
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public interface IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// 获取DbContext
    /// </summary>
    /// <returns></returns>
    Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default);
}