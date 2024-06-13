using Hx.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 实体执行组件
    /// </summary>
    public sealed partial class EntityExecutePart<TEntity>
        where TEntity : class, Internal.IPrivateEntity, new()
    {
        /// <summary>
        /// 获取实体同类（族群）
        /// </summary>
        /// <returns>DbSet{TEntity}</returns>
        public DbSet<TEntity> Ethnics()
        {
            return GetRepository().Entities;
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <returns>代理的实体</returns>
        public EntityEntry<TEntity> Insert()
        {
            return GetRepository().Insert(Entity);
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>代理的实体</returns>
        public Task<EntityEntry<TEntity>> InsertAsync( CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertNow()
        {
            return GetRepository().InsertNow(Entity);
        }

        /// <summary>
        /// 新增一条记录并立即提交
        /// </summary>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertNowAsync(Entity,cancellationToken);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> Update()
        {
            return GetRepository().Update(Entity);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateAsync()
        {
            return GetRepository().UpdateAsync(Entity);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateNow()
        {
            return GetRepository().UpdateNow(Entity);
        }

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateNowAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateInclude(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateInclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateInclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateIncludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateIncludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<string> propertyNames,CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates,CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateIncludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

      
        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateExclude(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> UpdateExclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateExclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateExcludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> UpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().UpdateExcludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<string> propertyNames,CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().UpdateExcludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> Delete()
        {
            return GetRepository().Delete(Entity);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> DeleteAsync()
        {
            return GetRepository().DeleteAsync(Entity);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> DeleteNow()
        {
            return GetRepository().DeleteNow(Entity);
        }

        /// <summary>
        /// 删除一条记录并立即提交
        /// </summary>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> DeleteNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().DeleteNowAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <param name="checkProperty"></param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdate(Expression<Func<TEntity, object>> checkProperty = null)
        {
            return GetRepository().InsertOrUpdate(Entity, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录
        /// </summary>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateAsync(Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateAsync(Entity, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="checkProperty"></param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateNow(Expression<Func<TEntity, object>> checkProperty = null)
        {
            return GetRepository().InsertOrUpdateNow(Entity, checkProperty);
        }

        /// <summary>
        /// 新增或更新一条记录并立即执行
        /// </summary>
        /// <param name="checkProperty"></param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateNowAsync(Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateNowAsync(Entity, checkProperty, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateInclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateInclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateIncludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateIncludeNow(Entity, propertyNames);
        }

       
        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateIncludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExclude(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExclude(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>代理中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<string> propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public EntityEntry<TEntity> InsertOrUpdateExcludeNow(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNow(Entity, propertyPredicates);
        }


        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(params string[] propertyNames)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(params Expression<Func<TEntity, object>>[] propertyPredicates)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyNames, cancellationToken);
        }

        /// <summary>
        /// 新增或更新一条排除特定属性记录并立即提交
        /// </summary>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        public Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default)
        {
            return GetRepository().InsertOrUpdateExcludeNowAsync(Entity, propertyPredicates, cancellationToken);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <returns></returns>
        public EntityEntry<TEntity> FakeDelete()
        {
            return GetRepository().FakeDelete(Entity);
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <returns></returns>
        public Task<EntityEntry<TEntity>> FakeDeleteAsync()
        {
            return GetRepository().FakeDeleteAsync(Entity);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <returns></returns>
        public EntityEntry<TEntity> FakeDeleteNow()
        {
            return GetRepository().FakeDeleteNow(Entity);
        }

        /// <summary>
        /// 假删除并立即提交
        /// </summary>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns></returns>
        public Task<EntityEntry<TEntity>> FakeDeleteNowAsync(CancellationToken cancellationToken = default)
        {
            return GetRepository().FakeDeleteNowAsync(Entity, cancellationToken);
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <returns></returns>
        private IPrivateRepository<TEntity> GetRepository()
        {
            return Penetrates.GetService(typeof(IRepository<,>).MakeGenericType(typeof(TEntity), DbContextLocator), ContextScoped) as IPrivateRepository<TEntity>;
        }
    }
}