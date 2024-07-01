using Starshine.EntityFrameworkCore.Extensions.LinqBuilder;
using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 可操作仓储分部类
    /// </summary>
    public abstract class OperableRepository<TEntity> : ReadOnlyRepository<TEntity>, IOperableRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// 工作单元管理
        /// </summary>
        protected readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public OperableRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<EntityEntry<TEntity>> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 插入多条实体
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity, cancellationToken: cancellationToken);
            }

            if (autoSave)
            {
                await SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 保存变更
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (_unitOfWorkManager?.Current != null)
            {
                return _unitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}