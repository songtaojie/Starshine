using Starshine.EntityFrameworkCore.Extensions.LinqBuilder;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;
using Starshine.Common;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 可写仓储分部类
    /// </summary>
    public abstract class ReadOnlyRepository<TEntity>: RepositoryBase,
        IReadOnlyRepository<TEntity>
        where TEntity :class, IEntity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scoped">服务提供器</param>
        public ReadOnlyRepository(IServiceProvider scoped) : base(scoped)
        {
        }

        /// <summary>
        /// 根据键查询一条记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库实体</returns>
        public virtual async Task<TEntity> FindAsync(object key, CancellationToken cancellationToken = default)
        {
            var entity = await FindOrDefaultAsync(key, cancellationToken);
            return entity ?? throw new InvalidOperationException("Sequence contains no elements.");
        }

        /// <summary>
        /// 根据键查询一条记录
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public abstract Task<TEntity?> FindOrDefaultAsync(object key, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<TEntity> FirstAsync(bool? tracking = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking);
            return await query.FirstAsync(cancellationToken);
        }

        /// <summary>
        /// 根据表达式查询一条记录
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, bool? tracking = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking);
            return await query.FirstAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(bool? tracking = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// 根据表达式查询一条记录
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool? tracking = null, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking);
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>bool</returns>
        public virtual async Task<bool> AnyAsync(bool? tracking = null, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking, ignoreQueryFilters);
            return await query.AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 根据表达式判断记录是否存在
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>bool</returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, bool? tracking = null, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking, ignoreQueryFilters);
            return await query.AnyAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 查看记录条数
        /// </summary>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>int</returns>
        public virtual async Task<int> CountAsync(bool? tracking = null, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking, ignoreQueryFilters);
            return await query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// 根据表达式查询记录条数
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>int</returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, bool? tracking = null, bool ignoreQueryFilters = false, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync(tracking, ignoreQueryFilters);
            return await query.CountAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 构建查询分析器
        /// </summary>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <returns>IQueryable{TEntity}</returns>
        public abstract Task<IQueryable<TEntity>> GetQueryableAsync(bool? tracking = null, bool ignoreQueryFilters = false);

        /// <summary>
        /// 构建查询分析器
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <returns>IQueryable{TEntity}</returns>
        private async Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>>? predicate, bool? tracking = null, bool ignoreQueryFilters = false)
        {
            var entities = await GetQueryableAsync(tracking);
            if (ignoreQueryFilters) entities = entities.IgnoreQueryFilters();
            if (predicate != null) entities = entities.Where(predicate);

            return entities;
        }

        /// <summary>
        /// 构建查询分析器
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <param name="tracking">是否跟踪实体</param>
        /// <param name="ignoreQueryFilters">是否忽略查询过滤器</param>
        /// <returns>IQueryable{TEntity}</returns>
        private async Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, int, bool>>? predicate, bool? tracking = null, bool ignoreQueryFilters = false)
        {
            var entities = await GetQueryableAsync(tracking);
            if (ignoreQueryFilters) entities = entities.IgnoreQueryFilters();
            if (predicate != null) entities = entities.Where(predicate);

            return entities;
        }
       
    }
}