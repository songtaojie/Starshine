﻿using Hx.Sdk.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// Sql 查询仓储分部类
    /// </summary>
    public partial class PrivateSqlRepository
    {
        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable</returns>
        public virtual DataTable SqlQuery(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteReader(sql, parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>DataTable</returns>
        public virtual DataTable SqlQuery(string sql, object model)
        {
            return Database.ExecuteReader(sql, model).dataTable;
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataTable}</returns>
        public virtual Task<DataTable> SqlQueryAsync(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteReaderAsync(sql, parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public virtual Task<DataTable> SqlQueryAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return Database.ExecuteReaderAsync(sql, parameters, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public virtual async Task<DataTable> SqlQueryAsync(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (dataTable, _) = await Database.ExecuteReaderAsync(sql, model, cancellationToken: cancellationToken);
            return dataTable;
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T}</returns>
        public virtual List<T> SqlQuery<T>(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteReader(sql, parameters).ToList<T>();
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>List{T}</returns>
        public virtual List<T> SqlQuery<T>(string sql, object model)
        {
            return Database.ExecuteReader(sql, model).dataTable.ToList<T>();
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{List{T}}</returns>
        public virtual async Task<List<T>> SqlQueryAsync<T>(string sql, params DbParameter[] parameters)
        {
            var dataTable = await Database.ExecuteReaderAsync(sql, parameters);
            return dataTable.ToList<T>();
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public virtual async Task<List<T>> SqlQueryAsync<T>(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            var dataTable = await Database.ExecuteReaderAsync(sql, parameters, cancellationToken: cancellationToken);
            return dataTable.ToList<T>();
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public virtual async Task<List<T>> SqlQueryAsync<T>(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (dataTable, _) = await Database.ExecuteReaderAsync(sql, model, cancellationToken: cancellationToken);
            return dataTable.ToList<T>();
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataSet</returns>
        public virtual DataSet SqlQueries(string sql, params DbParameter[] parameters)
        {
            return Database.DataAdapterFill(sql, parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>DataSet</returns>
        public virtual DataSet SqlQueries(string sql, object model)
        {
            return Database.DataAdapterFill(sql, model).dataSet;
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataSet}</returns>
        public virtual Task<DataSet> SqlQueriesAsync(string sql, params DbParameter[] parameters)
        {
            return Database.DataAdapterFillAsync(sql, parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public virtual Task<DataSet> SqlQueriesAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return Database.DataAdapterFillAsync(sql, parameters, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public virtual async Task<DataSet> SqlQueriesAsync(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (dataSet, _) = await Database.DataAdapterFillAsync(sql, model, cancellationToken: cancellationToken);
            return dataSet;
        }

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T1}</returns>
        public virtual List<T1> SqlQueries<T1>(string sql, params DbParameter[] parameters)
        {
            return Database.DataAdapterFill(sql, parameters).ToList<T1>();
        }

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{List{T1}}</returns>
        public virtual async Task<List<T1>> SqlQueriesAsync<T1>(string sql, params DbParameter[] parameters)
        {
            var dataset = await Database.DataAdapterFillAsync(sql, parameters);
            return dataset.ToList<T1>();
        }

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T1}}</returns>
        public virtual async Task<List<T1>> SqlQueriesAsync<T1>(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            var dataset = await Database.DataAdapterFillAsync(sql, parameters, cancellationToken: cancellationToken);
            return dataset.ToList<T1>();
        }
    }
}