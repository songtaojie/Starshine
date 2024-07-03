// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public interface IEFCoreRepository<TDbContext, TEntity>:IRepository
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    /// <summary>
    /// 获取数据库上下文
    /// </summary>
    /// <returns></returns>
    Task<TDbContext> GetDbContextAsync();

    /// <summary>
    /// 获取DbSet
    /// </summary>
    /// <returns></returns>
    Task<DbSet<TEntity>> GetDbSetAsync();
}
