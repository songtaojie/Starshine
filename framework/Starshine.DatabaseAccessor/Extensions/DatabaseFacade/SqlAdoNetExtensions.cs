﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// ADONET 拓展类
    /// </summary>
    public static class SqlAdoNetExtensions
    {
        /// <summary>
        /// 执行 Sql 返回 DataTable
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="behavior">行为</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteReader(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default)
        {
          
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = databaseFacade.PrepareDbCommand(sql, parameters, commandType);

            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(behavior);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataTable;
        }

        /// <summary>
        /// 执行 Sql 返回 DataTable
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="model">命令模型</param>
        /// <param name="behavior">行为</param>
        /// <returns>(DataTable dataTable, DbParameter[] dbParameters)</returns>
        public static (DataTable dataTable, DbParameter[] dbParameters) ExecuteReader(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default)
        {

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = databaseFacade.PrepareDbCommand(sql, model, commandType);

            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(behavior);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (dataTable, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 返回 DataTable
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="behavior">行为</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>DataTable</returns>
        public static async Task<DataTable> ExecuteReaderAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
          
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = await databaseFacade.PrepareDbCommandAsync(sql, parameters, commandType, cancellationToken);

            // 读取数据
            using var dbDataReader = await dbCommand.ExecuteReaderAsync(behavior, cancellationToken);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataTable;
        }

        /// <summary>
        /// 执行 Sql 返回 DataTable
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="behavior">行为</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(DataTable dataTable, DbParameter[] dbParameters)</returns>
        public static async Task<(DataTable dataTable, DbParameter[] dbParameters)> ExecuteReaderAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = await databaseFacade.PrepareDbCommandAsync(sql, model, commandType, cancellationToken);

            // 读取数据
            using var dbDataReader = await dbCommand.ExecuteReaderAsync(behavior, cancellationToken);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (dataTable, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 语句返回受影响行数
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = databaseFacade.PrepareDbCommand(sql, parameters, commandType);

            // 执行返回受影响行数
            var rowEffects = dbCommand.ExecuteNonQuery();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return rowEffects;
        }

        /// <summary>
        /// 执行 Sql 语句返回受影响行数
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(int rowEffects, DbParameter[] dbParameters)</returns>
        public static (int rowEffects, DbParameter[] dbParameters) ExecuteNonQuery(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = databaseFacade.PrepareDbCommand(sql, model, commandType);

            // 执行返回受影响行数
            var rowEffects = dbCommand.ExecuteNonQuery();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (rowEffects, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 语句返回受影响行数
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>受影响行数</returns>
        public static async Task<int> ExecuteNonQueryAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = await databaseFacade.PrepareDbCommandAsync(sql, parameters, commandType, cancellationToken);

            // 执行返回受影响行数
            var rowEffects = await dbCommand.ExecuteNonQueryAsync(cancellationToken);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return rowEffects;
        }

        /// <summary>
        /// 执行 Sql 语句返回受影响行数
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(int rowEffects, DbParameter[] dbParameters)</returns>
        public static async Task<(int rowEffects, DbParameter[] dbParameters)> ExecuteNonQueryAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = await databaseFacade.PrepareDbCommandAsync(sql, model, commandType, cancellationToken);

            // 执行返回受影响行数
            var rowEffects = await dbCommand.ExecuteNonQueryAsync(cancellationToken);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (rowEffects, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 返回单行单列的值
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>单行单列的值</returns>
        public static object ExecuteScalar(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = databaseFacade.PrepareDbCommand(sql, parameters, commandType);

            // 执行返回单行单列的值
            var result = dbCommand.ExecuteScalar();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return result != DBNull.Value ? result : default;
        }

        /// <summary>
        /// 执行 Sql 返回单行单列的值
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(object result, DbParameter[] dbParameters)</returns>
        public static (object result, DbParameter[] dbParameters) ExecuteScalar(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = databaseFacade.PrepareDbCommand(sql, model, commandType);

            // 执行返回单行单列的值
            var result = dbCommand.ExecuteScalar();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (result != DBNull.Value ? result : default, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 返回单行单列的值
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>单行单列的值</returns>
        public static async Task<object> ExecuteScalarAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = await databaseFacade.PrepareDbCommandAsync(sql, parameters, commandType, cancellationToken);

            // 执行返回单行单列的值
            var result = await dbCommand.ExecuteScalarAsync(cancellationToken);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return result != DBNull.Value ? result : default;
        }

        /// <summary>
        /// 执行 Sql 返回单行单列的值
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(object result, DbParameter[] dbParameters)</returns>
        public static async Task<(object result, DbParameter[] dbParameters)> ExecuteScalarAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = await databaseFacade.PrepareDbCommandAsync(sql, model, commandType, cancellationToken);

            // 执行返回单行单列的值
            var result = await dbCommand.ExecuteScalarAsync(cancellationToken);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (result != DBNull.Value ? result : default, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 返回 DataSet
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>DataSet</returns>
        public static DataSet DataAdapterFill(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            // 处理 Sqlite 不支持 DataSet 问题
            if (DbProvider.IsDatabaseFor(databaseFacade.ProviderName, DbProvider.Sqlite))
            {
                return SqliteDataSetFill(databaseFacade, sql, parameters, commandType);
            }

            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter) = databaseFacade.PrepareDbDataAdapter(sql, parameters, commandType);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataSet;
        }

        /// <summary>
        /// 执行 Sql 返回 DataSet
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>(DataSet dataSet, DbParameter[] dbParameters)</returns>
        public static (DataSet dataSet, DbParameter[] dbParameters) DataAdapterFill(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text)
        {
            // 处理 Sqlite 不支持 DataSet 问题
            if (DbProvider.IsDatabaseFor(databaseFacade.ProviderName, DbProvider.Sqlite))
            {
                return SqliteDataSetFill(databaseFacade, sql, model, commandType);
            }

            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter, dbParameters) = databaseFacade.PrepareDbDataAdapter(sql, model, commandType);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (dataSet, dbParameters);
        }

        /// <summary>
        /// 执行 Sql 返回 DataSet
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public static async Task<DataSet> DataAdapterFillAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 处理 Sqlite 不支持 DataSet 问题
            if (DbProvider.IsDatabaseFor(databaseFacade.ProviderName, DbProvider.Sqlite))
            {
                return await SqliteDataSetFillAsync(databaseFacade, sql, parameters, commandType, cancellationToken: cancellationToken);
            }

            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter) = await databaseFacade.PrepareDbDataAdapterAsync(sql, parameters, commandType, cancellationToken);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataSet;
        }

        /// <summary>
        /// 执行 Sql 返回 DataSet
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">命令模型</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>(DataSet dbSet, DbParameter[] dbParameters)</returns>
        public static async Task<(DataSet dbSet, DbParameter[] dbParameters)> DataAdapterFillAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 处理 Sqlite 不支持 DataSet 问题
            if (DbProvider.IsDatabaseFor(databaseFacade.ProviderName, DbProvider.Sqlite))
            {
                return await SqliteDataSetFillAsync(databaseFacade, sql, model, commandType, cancellationToken: cancellationToken);
            }

            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter, dbParameters) = await databaseFacade.PrepareDbDataAdapterAsync(sql, model, commandType, cancellationToken);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return (dataSet, dbParameters);
        }

        /// <summary>
        /// Sqlite DataSet Fill
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="behavior">行为</param>
        /// <returns>DataTable</returns>
        private static DataSet SqliteDataSetFill(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default)
        {
            var sqls = sql.Split(";", StringSplitOptions.RemoveEmptyEntries);

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = databaseFacade.PrepareDbCommand(string.Empty, parameters, commandType);

            // 执行多个 Sql
            var dataset = ExecuteSqlsForSqlite(behavior, sqls, dbCommand);

            return dataset;
        }

        /// <summary>
        /// Sqlite DataSet Fill
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="model">命令模型</param>
        /// <param name="behavior">行为</param>
        /// <returns>(DataTable dataTable, DbParameter[] dbParameters)</returns>
        private static (DataSet dataSet, DbParameter[] dbParameters) SqliteDataSetFill(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default)
        {
            var sqls = sql.Split(";", StringSplitOptions.RemoveEmptyEntries);

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = databaseFacade.PrepareDbCommand(string.Empty, model, commandType);

            // 执行多个 Sql
            var dataset = ExecuteSqlsForSqlite(behavior, sqls, dbCommand);

            return (dataset, dbParameters);
        }

        /// <summary>
        /// Sqlite DataSet Fill
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="behavior">行为</param>
        /// <param name="cancellationToken"></param>
        /// <returns>DataTable</returns>
        private static async Task<DataSet> SqliteDataSetFillAsync(this DatabaseFacade databaseFacade, string sql, DbParameter[] parameters = null, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {
            var sqls = sql.Split(";", StringSplitOptions.RemoveEmptyEntries);

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand) = await databaseFacade.PrepareDbCommandAsync(string.Empty, parameters, commandType, cancellationToken);

            // 执行多个 Sql
            var dataset = ExecuteSqlsForSqlite(behavior, sqls, dbCommand);

            return dataset;
        }

        /// <summary>
        /// Sqlite DataSet Fill
        /// </summary>
        /// <param name="databaseFacade">ADO.NET 数据库对象</param>
        /// <param name="sql">sql 语句</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="model">命令模型</param>
        /// <param name="behavior">行为</param>
        /// <param name="cancellationToken"></param>
        /// <returns>(DataTable dataTable, DbParameter[] dbParameters)</returns>
        private static async Task<(DataSet dataSet, DbParameter[] dbParameters)> SqliteDataSetFillAsync(this DatabaseFacade databaseFacade, string sql, object model, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
        {

            var sqls = sql.Split(";", StringSplitOptions.RemoveEmptyEntries);

            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, dbParameters) = await databaseFacade.PrepareDbCommandAsync(string.Empty, model, commandType, cancellationToken);

            // 执行多个 Sql
            var dataset = ExecuteSqlsForSqlite(behavior, sqls, dbCommand);

            return (dataset, dbParameters);
        }

        /// <summary>
        /// 执行 Sqlite 多个 Sql
        /// </summary>
        /// <param name="behavior"></param>
        /// <param name="sqls"></param>
        /// <param name="dbCommand"></param>
        /// <returns></returns>
        private static DataSet ExecuteSqlsForSqlite(CommandBehavior behavior, string[] sqls, DbCommand dbCommand)
        {
            var dataset = new DataSet();

            foreach (var itemSql in sqls)
            {
                dbCommand.CommandText = itemSql;

                // 读取数据
                using var dbDataReader = dbCommand.ExecuteReader(behavior);

                // 填充到 DataTable
                using var dataTable = new DataTable();
                dataTable.Load(dbDataReader);

                dataset.Tables.Add(dataTable);
            }

            // 清空命令参数
            dbCommand.Parameters.Clear();
            return dataset;
        }
    }
}