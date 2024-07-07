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
/// 
/// </summary>
public class EFCoreDatabaseApi : IDatabaseApi
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public DbContext StarterDbContext { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext"></param>
    public EFCoreDatabaseApi(DbContext dbContext)
    {
        StarterDbContext = dbContext;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return StarterDbContext.SaveChangesAsync(cancellationToken);
    }
}
