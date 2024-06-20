using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Transactions;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 工作单元配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnitOfWorkAttribute() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// <para>支持传入事务隔离级别 <see cref="IsolationLevel"/> 参数值</para>
        /// </remarks>
        /// <param name="isolationLevel">事务隔离级别</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// 如果此UOW是事务性的，则此选项指示事务的隔离级别。 如果未提供，则使用默认值。
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order => 9999;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取动作方法描述器
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if(actionDescriptor == null) return;
            var method = actionDescriptor.MethodInfo;
            // 是否启用分布式环境事务
            TransactionScope
            var transactionScope = UseAmbientTransaction
                 ? new TransactionScope(TransactionScope,
                new TransactionOptions { IsolationLevel = IsolationLevel, Timeout = TransactionTimeout > 0 ? TimeSpan.FromSeconds(TransactionTimeout) : default }
                , TransactionScopeAsyncFlow)
                 : default;

            // 创建日志记录器
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<UnitOfWork>>();

            // 创建分布式环境事务
            (var transactionScope, var logger) = CreateTransactionScope(context);

            try
            {
                // 打印工作单元开始消息
                if (UseAmbientTransaction) App.PrintToMiniProfiler(MiniProfilerCategory, "Beginning (Ambient)");

                // 开始事务
                BeginTransaction(context, method, out var _unitOfWork, out var unitOfWorkAttribute);

                // 获取执行 Action 结果
                var resultContext = await next();

                // 提交事务
                CommitTransaction(context, _unitOfWork, unitOfWorkAttribute, resultContext);

                // 提交分布式环境事务
                if (resultContext.Exception == null)
                {
                    transactionScope?.Complete();

                    // 打印事务提交消息
                    if (UseAmbientTransaction) App.PrintToMiniProfiler(MiniProfilerCategory, "Completed (Ambient)");
                }
                else
                {
                    // 打印事务回滚消息
                    if (UseAmbientTransaction) App.PrintToMiniProfiler(MiniProfilerCategory, "Rollback (Ambient)", isError: true);

                    logger.LogError(resultContext.Exception, "Transaction Failed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transaction Failed.");

                // 打印事务回滚消息
                if (UseAmbientTransaction) App.PrintToMiniProfiler(MiniProfilerCategory, "Rollback (Ambient)", isError: true);

                throw;
            }
            finally
            {
                transactionScope?.Dispose();
            }
        }
    }
}