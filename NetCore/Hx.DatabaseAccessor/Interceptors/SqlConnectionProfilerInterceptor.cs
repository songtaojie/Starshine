﻿using Hx.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库连接拦截分析器
    /// </summary>
    internal sealed class SqlConnectionProfilerInterceptor : DbConnectionInterceptor
    {
        /// <summary>
        /// MiniProfiler 分类名
        /// </summary>
        private const string MiniProfilerCategory = "connection";

        /// <summary>
        /// 是否打印数据库连接信息
        /// </summary>
        private readonly bool IsPrintDbConnectionInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlConnectionProfilerInterceptor()
        {
            IsPrintDbConnectionInfo = Penetrates.DbSettings.PrintDbConnectionInfo == true;
        }

        /// <summary>
        /// 拦截数据库连接
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="eventData">数据库连接事件数据</param>
        /// <param name="result">拦截结果</param>
        /// <returns></returns>
        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            // 打印数据库连接信息到 MiniProfiler
            PrintConnectionToMiniProfiler(connection, eventData);

            return base.ConnectionOpening(connection, eventData, result);
        }

        /// <summary>
        /// 拦截数据库连接
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="eventData">数据库连接事件数据</param>
        /// <param name="result">拦截器结果</param>
        /// <param name="cancellationToken">取消异步Token</param>
        /// <returns></returns>
        public override Task<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            // 打印数据库连接信息到 MiniProfiler
            PrintConnectionToMiniProfiler(connection, eventData);

            return base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }

        /// <summary>
        /// 打印数据库连接信息到 MiniProfiler
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="eventData">数据库连接事件数据</param>
        private void PrintConnectionToMiniProfiler(DbConnection connection, ConnectionEventData eventData)
        {
            // 打印连接信息消息
            Penetrates.PrintToMiniProfiler(MiniProfilerCategory, "Information", $"[Connection Id: {eventData.ConnectionId}] / [Database: {connection.Database}]{(IsPrintDbConnectionInfo ? $" / [Connection String: {connection.ConnectionString}]" : string.Empty)}");
        }
    }
}