using Starshine.EntityFrameworkCore.Extensions;
using Hx.Sdk.Extensions;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// Sql 执行仓储分部类
    /// </summary>
    public partial class PrivateSqlRepository
    {
        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public virtual int SqlNonQuery(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>int</returns>
        public virtual int SqlNonQuery(string sql, object model)
        {
            return Database.ExecuteNonQuery(sql, model).rowEffects;
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public virtual Task<int> SqlNonQueryAsync(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteNonQueryAsync(sql, parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public virtual Task<int> SqlNonQueryAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return Database.ExecuteNonQueryAsync(sql, parameters, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public virtual async Task<int> SqlNonQueryAsync(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (rowEffects, _) = await Database.ExecuteNonQueryAsync(sql, model, cancellationToken: cancellationToken);
            return rowEffects;
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public virtual object SqlScalar(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteScalar(sql, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>object</returns>
        public virtual object SqlScalar(string sql, object model)
        {
            return Database.ExecuteScalar(sql, model).result;
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public virtual Task<object> SqlScalarAsync(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteScalarAsync(sql, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public virtual Task<object> SqlScalarAsync(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return Database.ExecuteScalarAsync(sql, parameters, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public virtual async Task<object> SqlScalarAsync(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (result, _) = await Database.ExecuteScalarAsync(sql, model, cancellationToken: cancellationToken);
            return result;
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public virtual TResult SqlScalar<TResult>(string sql, params DbParameter[] parameters)
        {
            return Database.ExecuteScalar(sql, parameters).ChangeType<TResult>();
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>TResult</returns>
        public virtual TResult SqlScalar<TResult>(string sql, object model)
        {
            return Database.ExecuteScalar(sql, model).result.ChangeType<TResult>();
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public virtual async Task<TResult> SqlScalarAsync<TResult>(string sql, params DbParameter[] parameters)
        {
            var result = await Database.ExecuteScalarAsync(sql, parameters);
            return result.ChangeType<TResult>();
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public virtual async Task<TResult> SqlScalarAsync<TResult>(string sql, DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            var result = await Database.ExecuteScalarAsync(sql, parameters, cancellationToken: cancellationToken);
            return result.ChangeType<TResult>();
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public virtual async Task<TResult> SqlScalarAsync<TResult>(string sql, object model, CancellationToken cancellationToken = default)
        {
            var (result, _) = await Database.ExecuteScalarAsync(sql, model, cancellationToken: cancellationToken);
            return result.ChangeType<TResult>();
        }

    }
}