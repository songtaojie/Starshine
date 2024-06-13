﻿using Hx.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 可更新仓储分部类
    /// </summary>
    public partial class PrivateRepository<TEntity>
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> Update(TEntity entity)
        {
            var entityEntry = Entities.Update(entity);

            return entityEntry;
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <returns>Task</returns>
        public virtual Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            Update(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateNow(TEntity entity)
        {
            var entityEntry = Update(entity);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        public virtual void UpdateNow(IEnumerable<TEntity> entities)
        {
            Update(entities);
            SaveNow();
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateAsync(entity);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>Task</returns>
        public virtual async Task UpdateNowAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(entities);
            await SaveNowAsync(cancellationToken);
        }


        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateInclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 判断是非参数只有一个，且是一个匿名类型
            if (propertyPredicates?.Count() == 1 && propertyPredicates.ElementAt(0).Body is NewExpression newExpression)
            {
                var propertyNames = newExpression.Members.Select(u => u.Name);
                return UpdateInclude(entity, propertyNames);
            }
            else
            {
                var entityEntry = ChangeEntityState(entity, EntityState.Detached);
                foreach (var propertyPredicate in propertyPredicates)
                {
                    EntityPropertyEntry(entity, propertyPredicate).IsModified = true;
                }

                return entityEntry;
            }
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateInclude(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = ChangeEntityState(entity, EntityState.Detached);
            foreach (var propertyName in propertyNames)
            {
                EntityPropertyEntry(entity, propertyName).IsModified = true;
            }

            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> UpdateIncludeAsync(TEntity entity, IEnumerable<string> propertyNames)
        {
            return Task.FromResult(UpdateInclude(entity, propertyNames));
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> UpdateIncludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return Task.FromResult(UpdateInclude(entity, propertyPredicates));
        }
     
        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeNow(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = UpdateInclude(entity, propertyNames);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            var entityEntry = UpdateInclude(entity, propertyPredicates);
            SaveNow();
            return entityEntry;
        }
      
        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(TEntity entity, IEnumerable<string> propertyNames,  CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateIncludeAsync(entity, propertyNames);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateIncludeAsync(entity, propertyPredicates);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExclude(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = ChangeEntityState(entity, EntityState.Modified);
            foreach (var propertyName in propertyNames)
            {
                EntityPropertyEntry(entity, propertyName).IsModified = false;
            }

            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 判断是非参数只有一个，且是一个匿名类型
            if (propertyPredicates?.Count() == 1 && propertyPredicates.ElementAt(0).Body is NewExpression newExpression)
            {
                var propertyNames = newExpression.Members.Select(u => u.Name);
                return UpdateExclude(entity, propertyNames);
            }
            else
            {
                var entityEntry = ChangeEntityState(entity, EntityState.Modified);
                foreach (var propertyPredicate in propertyPredicates)
                {
                    EntityPropertyEntry(entity, propertyPredicate).IsModified = false;
                }

                return entityEntry;
            }
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> UpdateExcludeAsync(TEntity entity, IEnumerable<string> propertyNames)
        {
            return Task.FromResult(UpdateExclude(entity, propertyNames));
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual Task<EntityEntry<TEntity>> UpdateExcludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return Task.FromResult(UpdateExclude(entity, propertyPredicates));
        }


        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeNow(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = UpdateExclude(entity, propertyNames);
            SaveNow();
            return entityEntry;
        }
       
        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            var entityEntry = UpdateExclude(entity, propertyPredicates);
            SaveNow();
            return entityEntry;
        }
       
        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateExcludeAsync(entity, propertyNames);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

       
        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateExcludeAsync(entity, propertyPredicates);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

      
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExists(TEntity entity)
        {
            // 检查实体是否有效
            CheckEntityEffective(entity);

            return Update(entity);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExistsAsync(TEntity entity)
        {
            // 检查实体是否有效
            await CheckEntityEffectiveAsync(entity);

            return await UpdateAsync(entity);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExistsNow(TEntity entity)
        {
            var entityEntry = UpdateExists(entity);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExistsNowAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateExistsAsync(entity);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeExists(TEntity entity, IEnumerable<string> propertyNames)
        {
            // 检查实体是否有效
            CheckEntityEffective(entity);

            return UpdateInclude(entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeExists(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 检查实体是否有效
            CheckEntityEffective(entity);

            return UpdateInclude(entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeExistsAsync(TEntity entity, IEnumerable<string> propertyNames)
        {
            // 检查实体是否有效
            await CheckEntityEffectiveAsync(entity);

            return await UpdateIncludeAsync(entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeExistsAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 检查实体是否有效
            await CheckEntityEffectiveAsync(entity);

            return await UpdateIncludeAsync(entity, propertyPredicates);
        }
       
        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeExistsNow(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = UpdateIncludeExists(entity, propertyNames);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateIncludeExistsNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            var entityEntry = UpdateIncludeExists(entity, propertyPredicates);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeExistsNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateIncludeExistsAsync(entity, propertyNames);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

       
        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateIncludeExistsNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateIncludeExistsAsync(entity, propertyPredicates);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeExists(TEntity entity, IEnumerable<string> propertyNames)
        {
            // 检查实体是否有效
            CheckEntityEffective(entity);

            return UpdateExclude(entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeExists(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 检查实体是否有效
            CheckEntityEffective(entity);

            return UpdateExclude(entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeExistsAsync(TEntity entity, IEnumerable<string> propertyNames)
        {
            // 检查实体是否有效
            await CheckEntityEffectiveAsync(entity);

            return await UpdateExcludeAsync(entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeExistsAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            // 检查实体是否有效
            await CheckEntityEffectiveAsync(entity);

            return await UpdateExcludeAsync(entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeExistsNow(TEntity entity, IEnumerable<string> propertyNames)
        {
            var entityEntry = UpdateExcludeExists(entity, propertyNames);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public virtual EntityEntry<TEntity> UpdateExcludeExistsNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            var entityEntry = UpdateExcludeExists(entity, propertyPredicates);
            SaveNow();
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeExistsNowAsync(TEntity entity, IEnumerable<string> propertyNames,CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateExcludeExistsAsync(entity, propertyNames);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public virtual async Task<EntityEntry<TEntity>> UpdateExcludeExistsNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            var entityEntry = await UpdateExcludeExistsAsync(entity, propertyPredicates);
            await SaveNowAsync(cancellationToken);
            return entityEntry;
        }

        /// <summary>
        /// 检查实体是否有效
        /// </summary>
        /// <param name="entity">实体</param>
        private void CheckEntityEffective(TEntity entity)
        {
            var (_, Value) = GetEntityKeyValue(entity);
            var trackEntity = FindOrDefault(Value) ?? throw DbHelpers.DataNotFoundException();
            // 取消跟踪实体
            Detach(trackEntity);
        }

        /// <summary>
        /// 检查实体是否有效
        /// </summary>
        /// <param name="entity">实体</param>
        private async Task CheckEntityEffectiveAsync(TEntity entity)
        {
            var (_, Value) = GetEntityKeyValue(entity);
            var trackEntity = await FindOrDefaultAsync(Value);
            if (trackEntity == null) throw DbHelpers.DataNotFoundException();
            // 取消跟踪实体
            Detach(trackEntity);
        }

        /// <summary>
        /// 获取实体键和值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private (string Key, object Value) GetEntityKeyValue(TEntity entity)
        {
            // 读取主键
            var keyProperty = EntityType.FindPrimaryKey().Properties.AsEnumerable().FirstOrDefault()?.PropertyInfo;
            if (keyProperty == null) return default;

            // 获取主键的值
            var keyName = keyProperty.Name;
            var keyValue = keyProperty.GetValue(entity);

            // 主键不能为空，且不能为 0，也不能为 Guid 空值
            if (keyValue == null || (keyProperty.PropertyType.IsValueType && (keyValue.Equals(0) || keyValue.Equals(Guid.Empty)))) throw new InvalidOperationException("The primary key value is not set.");

            return (keyName, keyValue);
        }
    }
}