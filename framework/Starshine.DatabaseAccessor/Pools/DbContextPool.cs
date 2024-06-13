﻿using Hx.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库上下文池
    /// </summary>
    public class DbContextPool : IDbContextPool
    {

        /// <summary>
        /// MiniProfiler 组件状态
        /// </summary>
        private readonly bool EnabledMiniProfiler;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbContextPool()
        {
            EnabledMiniProfiler = Penetrates.DbSettings.EnabledMiniProfiler == true;

            dbContexts = new ConcurrentBag<DbContext>();
            failedDbContexts = new ConcurrentBag<DbContext>();
        }

        /// <summary>
        /// 线程安全的数据库上下文集合
        /// </summary>
        private readonly ConcurrentBag<DbContext> dbContexts;

        /// <summary>
        /// 登记错误的数据库上下文
        /// </summary>
        private readonly ConcurrentBag<DbContext> failedDbContexts;

        /// <summary>
        /// 获取所有数据库上下文
        /// </summary>
        /// <returns></returns>
        public ConcurrentBag<DbContext> GetDbContexts()
        {
            return dbContexts;
        }

        /// <summary>
        /// 保存数据库上下文
        /// </summary>
        /// <param name="dbContext"></param>
        public void AddToPool(DbContext dbContext)
        {
            // 排除已经存在的数据库上下文
            if (!dbContexts.Contains(dbContext))
            {
                dbContexts.Add(dbContext);
            }
        }

        /// <summary>
        /// 保存数据库上下文（异步）
        /// </summary>
        /// <param name="dbContext"></param>
        public Task AddToPoolAsync(DbContext dbContext)
        {
            AddToPool(dbContext);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 保存数据库上下文池中所有已更改的数据库上下文
        /// </summary>
        /// <returns></returns>
        public int SavePoolNow()
        {
            // 查找所有已改变的数据库上下文并保存更改
            return dbContexts
                .Where(u => u != null && u.ChangeTracker.HasChanges() && !failedDbContexts.Contains(u))
                .Select(u => u.SaveChanges()).Count();
        }

        /// <summary>
        /// 保存数据库上下文池中所有已更改的数据库上下文
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SavePoolNowAsync(CancellationToken cancellationToken = default)
        {
            // 查找所有已改变的数据库上下文并保存更改
            var tasks = dbContexts
                .Where(u => u != null && u.ChangeTracker.HasChanges() && !failedDbContexts.Contains(u))
                .Select(u => u.SaveChangesAsync(cancellationToken));

            // 等待所有异步完成
            var results = await Task.WhenAll(tasks);
            return results.Length;
        }

        /// <summary>
        /// 设置数据库上下文共享事务
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public void ShareTransaction(int skipCount, DbTransaction transaction)
        {
            // 跳过第一个数据库上下文并设置共享事务
            _ = dbContexts
                   .Where(u => u != null)
                   .Skip(skipCount)
                   .Select(u => u.Database.UseTransaction(transaction));
        }

        /// <summary>
        /// 设置数据库上下文共享事务
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="transaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ShareTransactionAsync(int skipCount, DbTransaction transaction, CancellationToken cancellationToken = default)
        {
            // 跳过第一个数据库上下文并设置贡献事务
            var tasks = dbContexts
                .Where(u => u != null)
                .Skip(skipCount)
                .Select(u => u.Database.UseTransactionAsync(transaction, cancellationToken));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 释放所有数据库上下文
        /// </summary>
        public void CloseAll()
        {
            if (!dbContexts.Any()) return;

            foreach (var dbContext in dbContexts)
            {
                var conn = dbContext.Database.GetDbConnection();
                if (conn.State == ConnectionState.Open)
                {
                    var wrapConn = EnabledMiniProfiler ? new ProfiledDbConnection(conn, MiniProfiler.Current) : conn;
                    wrapConn.Close();
                }
            }
        }
    }
}