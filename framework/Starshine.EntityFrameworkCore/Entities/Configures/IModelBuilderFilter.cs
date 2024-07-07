// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.DependencyInjection;

namespace Starshine.EntityFrameworkCore;

/// <summary>
///  数据库模型构建筛选器依赖接口
/// </summary>
/// <typeparam name="TDbContext">数据库上下文</typeparam>
public interface IModelBuilderFilter<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// 模型构建之前
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    void OnModelCreating(ModelBuilder modelBuilder, DbContext dbContext);

    /// <summary>
    /// 模型构建之后
    /// </summary>
    /// <param name="modelBuilder">模型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    void OnOnModelCreated(ModelBuilder modelBuilder, DbContext dbContext);
}