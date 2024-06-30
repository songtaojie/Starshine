// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// TransactionApi实现类
/// </summary>
public class EfCoreTransactionApi : ITransactionApi
{
    /// <summary>
    /// 事务
    /// </summary>
    public IDbContextTransaction DbContextTransaction { get; }
    /// <summary>
    /// 启动DbContext
    /// </summary>
    public DbContext StarterDbContext { get; }

    /// <summary>
    /// 存在的DbContext
    /// </summary>
    public List<DbContext> AttendedDbContexts { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextTransaction"></param>
    /// <param name="starterDbContext"></param>
    public EfCoreTransactionApi(
        IDbContextTransaction dbContextTransaction,
        DbContext starterDbContext)
    {
        DbContextTransaction = dbContextTransaction;
        StarterDbContext = starterDbContext;
        AttendedDbContexts = new List<DbContext>();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.Database.IsRelational() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue; //Relational databases use the shared transaction if they are using the same connection
            }

            await dbContext.Database.CommitTransactionAsync(cancellationToken);
        }

        await DbContextTransaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.Database.IsRelational() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue; //Relational databases use the shared transaction if they are using the same connection
            }

            await dbContext.Database.RollbackTransactionAsync(cancellationToken);
        }

        await DbContextTransaction.RollbackAsync(cancellationToken);
    }
}
