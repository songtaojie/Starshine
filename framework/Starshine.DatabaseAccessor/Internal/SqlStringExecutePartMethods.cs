using Starshine.DatabaseAccessor.Internal;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// 构建 Sql 字符串执行部件
    /// </summary>
    public sealed partial class SqlStringExecutePart
    {
        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable</returns>
        public DataTable SqlQuery(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQuery(SqlString, parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>DataTable</returns>
        public DataTable SqlQuery(object model)
        {
            return GetSqlRepository().SqlQuery(SqlString, model);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataTable}</returns>
        public Task<DataTable> SqlQueryAsync(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQueryAsync(SqlString, parameters);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public Task<DataTable> SqlQueryAsync(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueryAsync(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataTable}</returns>
        public Task<DataTable> SqlQueryAsync(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueryAsync(SqlString, model, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T}</returns>
        public List<T> SqlQuery<T>(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQuery<T>(SqlString, parameters);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <returns>List{T}</returns>
        public List<T> SqlQuery<T>(object model)
        {
            return GetSqlRepository().SqlQuery<T>(SqlString, model);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{List{T}}</returns>
        public Task<List<T>> SqlQueryAsync<T>(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQueryAsync<T>(SqlString, parameters);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public Task<List<T>> SqlQueryAsync<T>(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueryAsync<T>(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{List{T}}</returns>
        public Task<List<T>> SqlQueryAsync<T>(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueryAsync<T>(SqlString, model, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataSet</returns>
        public DataSet SqlQueries(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQueries(SqlString, parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>DataSet</returns>
        public DataSet SqlQueries(object model)
        {
            return GetSqlRepository().SqlQueries(SqlString, model);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task{DataSet}</returns>
        public Task<DataSet> SqlQueriesAsync(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQueriesAsync(SqlString, parameters);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public Task<DataSet> SqlQueriesAsync(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueriesAsync(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 DataSet
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task{DataSet}</returns>
        public Task<DataSet> SqlQueriesAsync(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlQueriesAsync(SqlString, model, cancellationToken);
        }

        /// <summary>
        ///  Sql 查询返回 List 集合
        /// </summary>
        /// <typeparam name="T1">返回类型</typeparam>
        /// <param name="parameters">命令参数</param>
        /// <returns>List{T1}</returns>
        public List<T1> SqlQueries<T1>(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlQueries<T1>(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public int SqlNonQuery(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlNonQuery(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>int</returns>
        public int SqlNonQuery(object model)
        {
            return GetSqlRepository().SqlNonQuery(SqlString, model);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>int</returns>
        public Task<int> SqlNonQueryAsync(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlNonQueryAsync(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public Task<int> SqlNonQueryAsync(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlNonQueryAsync(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 无数据返回
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>int</returns>
        public Task<int> SqlNonQueryAsync(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlNonQueryAsync(SqlString, model, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public object SqlScalar(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlScalar(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>object</returns>
        public object SqlScalar(object model)
        {
            return GetSqlRepository().SqlScalar(SqlString, model);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>object</returns>
        public Task<object> SqlScalarAsync(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlScalarAsync(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public Task<object> SqlScalarAsync(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlScalarAsync(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>object</returns>
        public Task<object> SqlScalarAsync(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlScalarAsync(SqlString, model, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public TResult SqlScalar<TResult>(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlScalar<TResult>(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>TResult</returns>
        public TResult SqlScalar<TResult>(object model)
        {
            return GetSqlRepository().SqlScalar<TResult>(SqlString, model);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <returns>TResult</returns>
        public Task<TResult> SqlScalarAsync<TResult>(params DbParameter[] parameters)
        {
            return GetSqlRepository().SqlScalarAsync<TResult>(SqlString, parameters);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public Task<TResult> SqlScalarAsync<TResult>(DbParameter[] parameters, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlScalarAsync<TResult>(SqlString, parameters, cancellationToken);
        }

        /// <summary>
        /// 执行 Sql 返回 单行单列
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>TResult</returns>
        public Task<TResult> SqlScalarAsync<TResult>(object model, CancellationToken cancellationToken = default)
        {
            return GetSqlRepository().SqlScalarAsync<TResult>(SqlString, model, cancellationToken);
        }

        /// <summary>
        /// 获取 Sql 执行仓储
        /// </summary>
        /// <returns></returns>
        private IPrivateSqlRepository GetSqlRepository()
        {
            return Penetrates.GetService(typeof(ISqlRepository<>).MakeGenericType(DbContextLocator), ContextScoped) as IPrivateSqlRepository;
        }
    }
}