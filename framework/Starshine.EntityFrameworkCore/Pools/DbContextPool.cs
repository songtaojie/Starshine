using Starshine.EntityFrameworkCore.Internal;
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
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文池
    /// </summary>
    public class DbContextPool : IDbContextPool
    {
        private readonly DbSettingsOptions _dbSettings;
        /// <summary>
        /// 线程安全的数据库上下文集合
        /// </summary>
        private readonly ConcurrentDictionary<string, DbContext> _dbContexts;

        /// <summary>
        /// 登记错误的数据库上下文
        /// </summary>
        private readonly ConcurrentDictionary<string, DbContext> _failedDbContexts;

        private bool _isRolledback;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DbContextPool(IOptionsSnapshot<DbSettingsOptions> options)
        {
            _dbSettings = options.Value;
            _dbContexts = new();
            _failedDbContexts = new();
        }

        /// <summary>
        /// 获取所有数据库上下文
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return _dbContexts.Values.ToImmutableList();
        }

        /// <summary>
        /// 保存数据库上下文（异步）
        /// </summary>
        /// <param name="dbContext"></param>
        public Task AddToPoolAsync(DbContext dbContext)
        {
            // 跳过非关系型数据库
            if (!dbContext.Database.IsRelational()) return Task.CompletedTask;
            var instanceId = dbContext.ContextId.InstanceId.ToString();
            if (!_dbContexts.TryAdd(instanceId, dbContext)) return Task.CompletedTask;
            // 订阅数据库上下文操作失败事件
            dbContext.SaveChangesFailed += (s, e) =>
            {
                // 排除已经存在的数据库上下文
                if (!_failedDbContexts.TryAdd(instanceId, dbContext)) return;

                // 当前事务
                DbContext? context = s as DbContext;
                if (context == null) return;
                var currentTransaction = context.Database.CurrentTransaction;

                // 只有事务不等于空且支持自动回滚
                if (!(currentTransaction != null && ((dynamic)context).FailedRollback == true)) return;
                //// 获取数据库连接信息
                //var connection = context.Database.GetDbConnection();

                // 回滚事务
                currentTransaction?.Rollback();
            };
            return Task.CompletedTask;
        }

        /// <summary>
        /// 保存数据库上下文池中所有已更改的数据库上下文
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback) return 0;

            // 查找所有已改变的数据库上下文并保存更改
            var tasks = _dbContexts
                .Where(u => u.Value != null && u.ChangeTracker.HasChanges() && !failedDbContexts.Contains(u))
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
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_isRolledback)
            {
                return;
            }

            _isRolledback = true;

            await RollbackAllAsync(cancellationToken);
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
                    var wrapConn = _dbSettings.EnabledMiniProfiler == true ? new ProfiledDbConnection(conn, MiniProfiler.Current) : conn;
                    wrapConn.Close();
                }
            }
        }
    }
}