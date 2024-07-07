using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// DatabaseFacade 拓展类
    /// </summary>
    public static class DbObjectExtensions
    {
        /// <summary>
        /// MiniProfiler 分类名
        /// </summary>
        private const string MiniProfilerCategory = "connection";

        /// <summary>
        /// 构造函数
        /// </summary>
        static DbObjectExtensions()
        {
        }

        /// <summary>
        /// 初始化数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand)</returns>
        public static (DbConnection dbConnection, DbCommand dbCommand) PrepareDbCommand(this DatabaseFacade databaseFacade, string sql, DbParameter[]? parameters = default, CommandType commandType = CommandType.Text)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            SetDbParameters(databaseFacade.ProviderName, ref dbCommand, parameters);

            // 打开数据库连接
            OpenConnection(databaseFacade, dbConnection);

            // 返回
            return (dbConnection, dbCommand);
        }

        /// <summary>
        /// 初始化数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters)</returns>
        public static (DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters) PrepareDbCommand(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            SetDbParameters(databaseFacade.ProviderName, ref dbCommand, model, out var dbParameters);

            // 打开数据库连接
            OpenConnection(databaseFacade, dbConnection);

            // 返回
            return (dbConnection, dbCommand, dbParameters);
        }

        /// <summary>
        /// 初始化数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand)</returns>
        public static async Task<(DbConnection dbConnection, DbCommand dbCommand)> PrepareDbCommandAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[]? parameters = default, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            SetDbParameters(databaseFacade.ProviderName, ref dbCommand, parameters);

            // 打开数据库连接
            await OpenConnectionAsync(databaseFacade, dbConnection, cancellationToken);

            // 返回
            return (dbConnection, dbCommand);
        }

        /// <summary>
        /// 初始化数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters)</returns>
        public static async Task<(DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters)> PrepareDbCommandAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            SetDbParameters(databaseFacade.ProviderName, ref dbCommand, model, out var dbParameters);

            // 打开数据库连接
            await OpenConnectionAsync(databaseFacade, dbConnection, cancellationToken);

            // 返回
            return (dbConnection, dbCommand, dbParameters);
        }

        /// <summary>
        /// 创建数据库命令对象
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DbConnection dbConnection, DbCommand dbCommand)</returns>
        private static (DbConnection dbConnection, DbCommand dbCommand) CreateDbCommand(this DatabaseFacade databaseFacade, string sql, CommandType commandType = CommandType.Text)
        {
            // 判断是否是关系型数据库
            if (!databaseFacade.IsRelational()) throw new InvalidOperationException("Only relational databases support this operations.");

            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

            // 检查是否支持存储过程
            DatabaseProviderHelper.CheckIsSupportedStoredProcedure(databaseFacade.ProviderName, commandType);

            // 判断是否启用 MiniProfiler 组件，如果有，则包装链接
            var dbConnection = DbContextHelper.GetDbConnection(databaseFacade);

            // 创建数据库命令对象
            var dbCommand = dbConnection.CreateCommand();

            // 设置基本参数
            dbCommand.Transaction = databaseFacade.CurrentTransaction?.GetDbTransaction();
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = sql;

            // 返回
            return (dbConnection, dbCommand);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="dbConnection">数据库连接对象</param>
        private static void OpenConnection(DatabaseFacade databaseFacade, DbConnection dbConnection)
        {
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();

                // 打印数据库连接信息到 MiniProfiler
                PrintConnectionToMiniProfiler(databaseFacade, dbConnection);
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="dbConnection">数据库连接对象</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        private static async Task OpenConnectionAsync(DatabaseFacade databaseFacade, DbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync(cancellationToken);

                // 打印数据库连接信息到 MiniProfiler
                PrintConnectionToMiniProfiler(databaseFacade, dbConnection);
            }
        }

        /// <summary>
        /// 设置数据库命令对象参数
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <param name="parameters">命令参数</param>
        private static void SetDbParameters(string? providerName, ref DbCommand dbCommand, DbParameter[]? parameters = default)
        {
            if (parameters == null || parameters.Length == 0) return;

            // 添加命令参数前缀
            foreach (var parameter in parameters)
            {
                parameter.ParameterName = DbParameterHelper.FixSqlParameterPlaceholder(providerName, parameter.ParameterName);
                dbCommand.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// 设置数据库命令对象参数
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <param name="model">参数模型</param>
        /// <param name="dbParameters">命令参数</param>
        private static void SetDbParameters(string? providerName, ref DbCommand dbCommand, object model, out DbParameter[] dbParameters)
        {
            dbParameters = DbParameterHelper.ConvertToDbParameters(model, dbCommand);
            SetDbParameters(providerName, ref dbCommand, dbParameters);
        }

        /// <summary>
        /// 打印数据库连接信息到 MiniProfiler
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="dbConnection">数据库连接对象</param>
        private static void PrintConnectionToMiniProfiler(DatabaseFacade databaseFacade, DbConnection dbConnection)
        {
            var connectionId = databaseFacade.GetService<IRelationalConnection>()?.ConnectionId;
            // 打印连接信息消息
            DbContextHelper.PrintToMiniProfiler(MiniProfilerCategory, "Information", $"[Connection Id: {connectionId}] / [Database: {dbConnection.Database}] / [Connection String: {dbConnection.ConnectionString}]");
        }
    }
}