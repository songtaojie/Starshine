using Starshine.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// 可插入仓储分部类
    /// </summary>
    public partial class PrivateRepository<TEntity>
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理的实体</returns>
        public virtual EntityEntry<TEntity> Insert(TEntity entity)
        {
            var entryEntity = Entities.Add(entity);

            return entryEntity;
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await Entities.AddAsync(entity, cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns></returns>
        public virtual Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Entities.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>数据库中返回的实体</returns>
        public virtual EntityEntry<TEntity> InsertNow(TEntity entity)
        {
            var entityEntry = Insert(entity);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void InsertNow(IEnumerable<TEntity> entities)
        {
            Insert(entities);
            SaveNow();
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中返回的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> InsertNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await InsertAsync(entity, cancellationToken);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 新增多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>Task</returns>
        public virtual async Task InsertNowAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await InsertAsync(entities, cancellationToken);
            await SaveNowAsync(cancellationToken);
        }
    }
}