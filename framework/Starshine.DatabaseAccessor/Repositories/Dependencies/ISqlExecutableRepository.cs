using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// Sql 执行仓储接口
    /// </summary>
    public interface ISqlExecutableRepository : ISqlExecutableRepository<MasterDbContextLocator>
    {
    }

    /// <summary>
    /// Sql 执行仓储接口
    /// </summary>
    /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
    public interface ISqlExecutableRepository<TDbContextLocator> : IPrivateSqlExecutableRepository
        where TDbContextLocator : class, IDbContextLocator
    {
    }

    /// <summary>
    /// Sql 执行仓储接口
    /// </summary>
    public interface IPrivateSqlExecutableRepository : IPrivateRootRepository
    {
        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        int SqlNonQuery(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>int</returns>
        int SqlNonQuery(string sql, object model);

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        Task<int> SqlNonQueryAsync(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        Task<int> SqlNonQueryAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        Task<int> SqlNonQueryAsync(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        object SqlScalar(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>object</returns>
        object SqlScalar(string sql, object model);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        Task<object> SqlScalarAsync(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        Task<object> SqlScalarAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        Task<object> SqlScalarAsync(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        TResult SqlScalar<TResult>(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>TResult</returns>
        TResult SqlScalar<TResult>(string sql, object model);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        Task<TResult> SqlScalarAsync<TResult>(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        Task<TResult> SqlScalarAsync<TResult>(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        Task<TResult> SqlScalarAsync<TResult>(string sql, object model, CancellationToken cancellationToken = default);
    }
}