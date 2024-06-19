using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 可删除仓储分部类
    /// </summary>
    public partial class PrivateRepository<TEntity>
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> Delete(TEntity entity)
        {
            return Entities.Remove(entity);
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Delete(entity));
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <returns>Task</returns>
        public virtual Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            Delete(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> DeleteNow(TEntity entity)
        {
            var entityEntry = Delete(entity);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 删除多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void DeleteNow(IEnumerable<TEntity> entities)
        {
            Delete(entities);
            SaveNow();
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> DeleteNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await DeleteAsync(entity);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }


        /// <summary>
        /// 删除多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>Task</returns>
        public virtual async Task DeleteNowAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(entities);
            await SaveNowAsync(cancellationToken);
        }

        /// <summary>
        /// 根据主键删除一条记录
        /// </summary>
        /// <param name="key">主键</param>
        public virtual void Delete(object key)
        {
            var deletedEntity = BuildDeletedEntity(key);
            if (deletedEntity != null) return;

            // 如果主键不存在，则采用 Find 查询
            var entity = FindOrDefault(key);
            if (entity != null) Delete(entity);
        }

        /// <summary>
        /// 根据主键删除一条记录
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task</returns>
        public virtual async Task DeleteAsync(object key, CancellationToken cancellationToken = default)
        {
            var deletedEntity = BuildDeletedEntity(key);
            if (deletedEntity != null) return;

            // 如果主键不存在，则采用 FindAsync 查询
            var entity = await FindOrDefaultAsync(key, cancellationToken);
            if (entity != null) await DeleteAsync(entity);
        }

        /// <summary>
        /// 根据主键删除一条记录并立即提交
        /// </summary>
        /// <param name="key">主键</param>
        public virtual void DeleteNow(object key)
        {
            Delete(key);
            SaveNow();
        }

        /// <summary>
        /// 根据主键删除一条记录并立即提交
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public virtual async Task DeleteNowAsync(object key, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(key, cancellationToken);
            await SaveNowAsync(cancellationToken);
        }

        /// <summary>
        /// 根据主键删除一条记录
        /// </summary>
        /// <param name="key">主键</param>
        public virtual void DeleteExists(object key)
        {
            var entity = FindOrDefault(key) ?? throw DbHelpers.DataNotFoundException();
            Delete(entity);
        }

        /// <summary>
        /// 根据主键删除一条记录
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task</returns>
        public virtual async Task DeleteExistsAsync(object key, CancellationToken cancellationToken = default)
        {
            var entity = await FindOrDefaultAsync(key, cancellationToken);
            if (entity == null) throw DbHelpers.DataNotFoundException();

            await DeleteAsync(entity);
        }

        /// <summary>
        /// 根据主键删除一条记录并立即提交
        /// </summary>
        /// <param name="key">主键</param>
        public virtual void DeleteExistsNow(object key)
        {
            DeleteExists(key);
            SaveNow();
        }

        /// <summary>
        /// 根据主键删除一条记录并立即提交
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public virtual async Task DeleteExistsNowAsync(object key, CancellationToken cancellationToken = default)
        {
            await DeleteExistsAsync(key, cancellationToken);
            await SaveNowAsync(cancellationToken);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual EntityEntry<TEntity> FakeDelete(TEntity entity)
        {
            var fakeDeleteProperty = SetFakePropertyValue(entity);
            return UpdateInclude(entity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual Task<EntityEntry<TEntity>> FakeDeleteAsync(TEntity entity)
        {
            var fakeDeleteProperty = SetFakePropertyValue(entity);
            return UpdateIncludeAsync(entity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public virtual EntityEntry<TEntity> FakeDeleteNow(TEntity entity)
        {
            var fakeDeleteProperty = SetFakePropertyValue(entity);
            return UpdateIncludeNow(entity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public virtual Task<EntityEntry<TEntity>> FakeDeleteNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var fakeDeleteProperty = SetFakePropertyValue(entity);
            return UpdateIncludeNowAsync(entity, new[] { fakeDeleteProperty.Name }, cancellationToken);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public virtual EntityEntry<TEntity> FakeDelete(object key)
        {
            var deletedEntity = BuildDeletedEntity(key, false);
            if (deletedEntity == null) return default;

            var fakeDeleteProperty = SetFakePropertyValue(deletedEntity);
            return UpdateInclude(deletedEntity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public virtual Task<EntityEntry<TEntity>> FakeDeleteAsync(object key)
        {
            var deletedEntity = BuildDeletedEntity(key, false);
            if (deletedEntity == null) return default;

            var fakeDeleteProperty = SetFakePropertyValue(deletedEntity);
            return UpdateIncludeAsync(deletedEntity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual EntityEntry<TEntity> FakeDeleteNow(object key)
        {
            var deletedEntity = BuildDeletedEntity(key, false);
            if (deletedEntity == null) return default;

            var fakeDeleteProperty = SetFakePropertyValue(deletedEntity);
            return UpdateIncludeNow(deletedEntity, new[] { fakeDeleteProperty.Name });
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public virtual Task<EntityEntry<TEntity>> FakeDeleteNowAsync(object key, CancellationToken cancellationToken = default)
        {
            var deletedEntity = BuildDeletedEntity(key, false);
            if (deletedEntity == null) return default;

            var fakeDeleteProperty = SetFakePropertyValue(deletedEntity);
            return UpdateIncludeNowAsync(deletedEntity, new[] { fakeDeleteProperty.Name }, cancellationToken);
        }

        /// <summary>
        /// 构建被删除的实体
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="isRealDelete">是否真删除</param>
        /// <returns></returns>
        private TEntity BuildDeletedEntity(object key, bool isRealDelete = true)
        {
            // 读取主键
            var keyProperty = EntityType.FindPrimaryKey().Properties.AsEnumerable().FirstOrDefault()?.PropertyInfo;
            if (keyProperty == null) return default;

            // 创建实体对象并设置主键值
            var entity = Activator.CreateInstance<TEntity>();
            keyProperty.SetValue(entity, key);

            if (isRealDelete)
            {
                // 设置实体状态为已删除
                ChangeEntityState(entity, EntityState.Deleted);
            }

            return entity;
        }

        /// <summary>
        /// 获取假删除的属性信息
        /// </summary>
        /// <returns></returns>
        private PropertyInfo SetFakePropertyValue(TEntity entity)
        {
            // 查找加删除特性
            var fakeDeleteProperty = EntityType.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(u => u.IsDefined(typeof(FakeDeleteAttribute), true));

            if (fakeDeleteProperty == null) throw new InvalidOperationException("No attributes marked as fake deleted were found");

            // 读取假删除的名和属性
            var fakeDeleteAttribute = fakeDeleteProperty.GetCustomAttribute<FakeDeleteAttribute>(true);
            var state = fakeDeleteAttribute.State;

            // 设置属性值
            fakeDeleteProperty.SetValue(entity, state);

            return fakeDeleteProperty;
        }
    }
}