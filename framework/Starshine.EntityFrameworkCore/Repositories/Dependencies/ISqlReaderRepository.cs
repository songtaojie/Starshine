using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// Sql 查询仓储接口
    /// </summary>
    public interface ISqlReaderRepository : ISqlReaderRepository<DefaultDbContextTypeProvider>
    {
    }

    /// <summary>
    /// Sql 查询仓储接口
    /// </summary>
    /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
    public interface ISqlReaderRepository<TDbContextLocator> : IPrivateSqlReaderRepository
        where TDbContextLocator : class, IDbContextLocator
    {
    }

    /// <summary>
    /// Sql 查询仓储接口
    /// </summary>
    public interface IPrivateSqlReaderRepository : IPrivateRootRepository
    {
        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable</returns>
        DataTable SqlQuery(string sql, params DbParameter[] parameters);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>DataTable</returns>
        DataTable SqlQuery(string sql, object model);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataTable}</returns>
        Task<DataTable> SqlQueryAsync(string sql, params DbParameter[] parameters);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        Task<DataTable> SqlQueryAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        Task<DataTable> SqlQueryAsync(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T}</returns>
        List<T> SqlQuery<T>(string sql, params DbParameter[] parameters);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>List{T}</returns>
        List<T> SqlQuery<T>(string sql, object model);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{List{T}}</returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, params DbParameter[] parameters);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataSet</returns>
        DataSet SqlQueries(string sql, params DbParameter[] parameters);

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>DataSet</returns>
        DataSet SqlQueries(string sql, object model);

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataSet}</returns>
        Task<DataSet> SqlQueriesAsync(string sql, params DbParameter[] parameters);

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        Task<DataSet> SqlQueriesAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        Task<DataSet> SqlQueriesAsync(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T1}</returns>
        List<T1> SqlQueries<T1>(string sql, params DbParameter[] parameters);

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>List{T1}</returns>
        List<T1> SqlQueries<T1>(string sql, object model);

    }
}