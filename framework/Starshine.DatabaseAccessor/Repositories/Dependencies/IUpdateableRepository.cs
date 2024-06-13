using Starshine.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// 可更新仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public partial interface IUpdateableRepository<TEntity> : IUpdateableRepository<TEntity, MasterDbContextLocator>
        where TEntity : class, IPrivateEntity, new()
    {
    }

    /// <summary>
    /// 可更新仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
    public partial interface IUpdateableRepository<TEntity, TDbContextLocator> : IPrivateUpdateableRepository<TEntity>
        where TEntity : class, IPrivateEntity, new()
        where TDbContextLocator : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 可更新仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public partial interface IPrivateUpdateableRepository<TEntity> : IPrivateRootRepository
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> Update(TEntity entity);

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity);

        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <returns>Task</returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateNow(TEntity entity);

        /// <summary>
        /// 更新多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        void UpdateNow(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateNowAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新多条记录并立即提交
        /// </summary>
        /// <param name="entities">多个实体</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>Task</returns>
        Task UpdateNowAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateInclude(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateInclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeAsync(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeNow(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExclude(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeAsync(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);


        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeNow(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExists(TEntity entity);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExistsAsync(TEntity entity);

        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExistsNow(TEntity entity);


        /// <summary>
        /// 更新一条记录并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExistsNowAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeExists(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeExists(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeExistsAsync(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeExistsAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeExistsNow(TEntity entity, IEnumerable<string> propertyNames);


        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateIncludeExistsNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeExistsNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新一条记录中的特定属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateIncludeExistsNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeExists(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录并排除属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeExists(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);


        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeExistsAsync(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录中的特定属性
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>代理中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeExistsAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeExistsNow(TEntity entity, IEnumerable<string> propertyNames);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <returns>数据库中的实体</returns>
        EntityEntry<TEntity> UpdateExcludeExistsNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyNames">属性名</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeExistsNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新一条记录并排除属性并立即提交
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="propertyPredicates">属性表达式</param>
        /// <param name="cancellationToken">取消异步令牌</param>
        /// <returns>数据库中的实体</returns>
        Task<EntityEntry<TEntity>> UpdateExcludeExistsNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);
    }
}