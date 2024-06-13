﻿using Hx.DatabaseAccessor.Internal;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    internal sealed class UnitOfWorkFilter : IAsyncActionFilter, IOrderedFilter
    {
        /// <summary>
        /// MiniProfiler 分类名
        /// </summary>
        private const string MiniProfilerCategory = "unitOfWork";

        /// <summary>
        /// 排序属性
        /// </summary>
        public int Order => 9999;

        /// <summary>
        /// 数据库上下文池
        /// </summary>
        private readonly IDbContextPool _dbContextPool;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextPool"></param>
        public UnitOfWorkFilter(IDbContextPool dbContextPool)
        {
            _dbContextPool = dbContextPool;
        }

        /// <summary>
        /// 拦截请求
        /// </summary>
        /// <param name="context">动作方法上下文</param>
        /// <param name="next">中间件委托</param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取动作方法描述器
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var method = actionDescriptor.MethodInfo;

            // 判断是否贴有工作单元特性
            if (!method.IsDefined(typeof(UnitOfWorkAttribute), true))
            {
                // 调用方法
                var resultContext = await next();

                // 判断是否异常
                if (resultContext.Exception == null) _dbContextPool.SavePoolNow();
            }
            else
            {
                // 打印事务开始消息
                Penetrates.PrintToMiniProfiler(MiniProfilerCategory, "Beginning");

                var dbContexts = _dbContextPool.GetDbContexts();
                IDbContextTransaction dbContextTransaction;

                // 判断 dbContextPool 中是否包含DbContext，如果是，则使用第一个数据库上下文开启事务，并应用于其他数据库上下文
                if (dbContexts.Any())
                {
                    // 先判断是否已经有上下文开启了事务
                    var transactionDbContext = dbContexts.FirstOrDefault(u => u.Database.CurrentTransaction != null);
                    if (transactionDbContext != null)
                    {
                        dbContextTransaction = transactionDbContext.Database.CurrentTransaction;
                    }
                    else
                    {
                        // 如果没有任何上下文有事务，则将第一个开启事务
                        dbContextTransaction = dbContexts.First().Database.BeginTransaction();
                    }

                    // 共享事务
                    _dbContextPool.ShareTransaction(1, dbContextTransaction.GetDbTransaction());
                }
                // 创建临时数据库上下文
                else
                {
                    var defaultDbContextLocator = Penetrates.DbContextDescriptors.LastOrDefault();

                    var newDbContext = Db.GetDbContext(defaultDbContextLocator.Key);

                    // 开启事务
                    dbContextTransaction = newDbContext.Database.BeginTransaction();
                    _dbContextPool.ShareTransaction(1, dbContextTransaction.GetDbTransaction());
                }

                // 调用方法
                var resultContext = await next();

                // 判断是否异常

                if (resultContext.Exception == null)
                {
                    try
                    {
                        // 将所有数据库上下文修改 SaveChanges();
                        var hasChangesCount = _dbContextPool.SavePoolNow();

                        // 提交共享事务
                        dbContextTransaction?.Commit();

                        // 打印事务提交消息
                        Penetrates.PrintToMiniProfiler(MiniProfilerCategory, "Completed", $"Transaction Completed! Has {hasChangesCount} DbContext Changes.");
                    }
                    catch
                    {
                        // 回滚事务
                        if (dbContextTransaction.GetDbTransaction().Connection != null) dbContextTransaction?.Rollback();

                        // 打印事务回滚消息
                        Penetrates.PrintToMiniProfiler(MiniProfilerCategory, "Rollback", isError: true);

                        throw;
                    }
                    finally
                    {
                        if (dbContextTransaction.GetDbTransaction().Connection != null) dbContextTransaction?.Dispose();
                    }
                }
                else
                {
                    // 回滚事务
                    if (dbContextTransaction.GetDbTransaction().Connection != null) dbContextTransaction?.Rollback();
                    dbContextTransaction?.Dispose();

                    // 打印事务回滚消息
                    Penetrates.PrintToMiniProfiler(MiniProfilerCategory, "Rollback", isError: true);
                }
            }

            // 手动关闭
            _dbContextPool.CloseAll();
        }
    }
}