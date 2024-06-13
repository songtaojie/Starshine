using Starshine.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.Sdk.Extensions
{
    /// <summary>
    /// Sql 字符串执行拓展类
    /// </summary>
    public static class SqlStringExecuteExtensions
    {
        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <typeparam name="TDbContextLocator"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlStringExecutePart Change<TDbContextLocator>(this string sql)
            where TDbContextLocator : class, IDbContextLocator
        {
            return new SqlStringExecutePart().SetSqlString(sql).Change<TDbContextLocator>();
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public static SqlStringExecutePart Change(this string sql, Type dbContextLocator)
        {
            return new SqlStringExecutePart().SetSqlString(sql).Change(dbContextLocator);
        }

        /// <summary>
        /// 设置数据库执行作用域
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="scoped"></param>
        /// <returns></returns>
        public static SqlStringExecutePart SetContextScoped(this string sql, IServiceProvider scoped)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SetContextScoped(scoped);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable</returns>
        public static DataTable SqlQuery(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQuery(parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <returns>DataTable</returns>
        public static DataTable SqlQuery(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQuery(model);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataTable}</returns>
        public static Task<DataTable> SqlQueryAsync(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync(parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public static Task<DataTable> SqlQueryAsync(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync(parameters, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public static Task<DataTable> SqlQueryAsync(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync(model, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql"></param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T}</returns>
        public static List<T> SqlQuery<T>(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQuery<T>(parameters);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql"></param>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <returns>List{T}</returns>
        public static List<T> SqlQuery<T>(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQuery<T>(model);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{List{T}}</returns>
        public static Task<List<T>> SqlQueryAsync<T>(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync<T>(parameters);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public static Task<List<T>> SqlQueryAsync<T>(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync<T>(parameters, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public static Task<List<T>> SqlQueryAsync<T>(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueryAsync<T>(model, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataSet</returns>
        public static DataSet SqlQueries(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueries(parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <returns>DataSet</returns>
        public static DataSet SqlQueries(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueries(model);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataSet}</returns>
        public static Task<DataSet> SqlQueriesAsync(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueriesAsync(parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public static Task<DataSet> SqlQueriesAsync(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueriesAsync(parameters, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public static Task<DataSet> SqlQueriesAsync(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueriesAsync(model, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T1}</returns>
        public static List<T1> SqlQueries<T1>(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlQueries<T1>(parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public static int SqlNonQuery(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlNonQuery(parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <returns>int</returns>
        public static int SqlNonQuery(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlNonQuery(model);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public static Task<int> SqlNonQueryAsync(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlNonQueryAsync(parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public static Task<int> SqlNonQueryAsync(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlNonQueryAsync(parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public static Task<int> SqlNonQueryAsync(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlNonQueryAsync(model, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public static object SqlScalar(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalar(parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <returns>object</returns>
        public static object SqlScalar(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalar(model);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public static Task<object> SqlScalarAsync(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync(parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public static Task<object> SqlScalarAsync(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync(parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public static Task<object> SqlScalarAsync(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync(model, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public static TResult SqlScalar<TResult>(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalar<TResult>(parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <returns>TResult</returns>
        public static TResult SqlScalar<TResult>(this string sql, object model)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalar<TResult>(model);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public static Task<TResult> SqlScalarAsync<TResult>(this string sql, params DbParameter[] parameters)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync<TResult>(parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public static Task<TResult> SqlScalarAsync<TResult>(this string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync<TResult>(parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public static Task<TResult> SqlScalarAsync<TResult>(this string sql, object model, CancellationToken cancellationToken = default)
        {
            return new SqlStringExecutePart().SetSqlString(sql).SqlScalarAsync<TResult>(model, cancellationToken);
        }

    }
}