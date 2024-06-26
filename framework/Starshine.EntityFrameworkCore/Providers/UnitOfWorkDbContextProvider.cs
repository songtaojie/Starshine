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
/// 默认实现
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    public Task<TDbContext> GetDbContextAsync()
    {
        throw new NotImplementedException();
    }
}
