using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 可插入仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public partial interface IInsertableRepository<TEntity> : IInsertableRepository<TEntity, MasterDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
    {
    }

    /// <summary>
    /// 可插入仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
    public partial interface IInsertableRepository<TEntity, TDbContextLocator> : IPrivateInsertableRepository<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 可插入仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IPrivateInsertableRepository<TEntity> : IPrivateRootRepository
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理的实体</returns>
        EntityEntry<TEntity> Insert(TEntity entity);

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理的实体</returns>
        Task<EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns></returns>
        Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>数据库中返回的实体</returns>
        EntityEntry<TEntity> InsertNow(TEntity entity);

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        void InsertNow(IEnumerable<TEntity> entities);

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中返回的实体</returns>
        Task<EntityEntry<TEntity>> InsertNowAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>Task</returns>
        Task InsertNowAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}