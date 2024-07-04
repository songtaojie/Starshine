using Starshine.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Common;
using System.Diagnostics.CodeAnalysis;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 可操作性仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IOperableRepository<TEntity>:IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// 插入一个新实体.
        /// </summary>
        /// <param name="autoSave">
        ///设置为true将自动保存更改到数据库
        /// </param>
        /// <param name="cancellationToken"> <see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        /// <param name="entity">插入实体</param>
        Task<EntityEntry<TEntity>> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        /// 插入多个新实体。
        /// </summary>
        /// <param name="autoSave">
        /// 设置为true将自动保存更改到数据库
        /// </param>
        /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        /// <param name="entities">要插入的实体。</param>
        /// <returns> <see cref="Task"/>.</returns>
        Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);


        /// <summary>
        ///更新现有实体。
        /// </summary>
        /// <param name="autoSave"> 设置为true将自动保存更改到数据库 </param>
        /// <param name="cancellationToken"> <see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <param name="entities">Entities to be updated.</param>
        /// <param name="autoSave"> 设置为true将自动保存更改到数据库 </param>
        /// <param name="cancellationToken"> <see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        /// <returns>Awaitable <see cref="Task"/>.</returns>
        Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除实体。
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave"> 设置为true将自动保存更改到数据库 </param>
        /// <param name="cancellationToken"> <see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除多个实体.
        /// </summary>
        /// <param name="entities">要删除的实体.</param>
        /// <param name="autoSave"> 设置为true将自动保存更改到数据库 </param>
        /// <param name="cancellationToken"> <see cref="T:System.Threading.CancellationToken" /> 在等待任务完成时观察。</param>
        /// <returns> <see cref="Task"/>.</returns>
        Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="checkProperty"></param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdate(TEntity entity, Expression<Func<TEntity, object>> checkProperty = null);

        ///// <summary>
        ///// 新增或更新一条记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="checkProperty"></param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateAsync(TEntity entity, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条记录并立即执行
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="checkProperty"></param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateNow(TEntity entity, Expression<Func<TEntity, object>> checkProperty = null);

        ///// <summary>
        ///// 新增或更新一条记录并立即执行
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="checkProperty"></param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateNowAsync(TEntity entity, Expression<Func<TEntity, object>> checkProperty = null, CancellationToken cancellationToken = default);


        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateInclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateInclude(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateInclude(TEntity entity, IEnumerable<string> propertyNames);

        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);


        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateIncludeNow(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateIncludeNow(TEntity entity, IEnumerable<string> propertyNames);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateIncludeNow(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateIncludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateIncludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExclude(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExclude(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExclude(TEntity entity, IEnumerable<string> propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExclude(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);


        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>代理中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);



        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExcludeNow(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExcludeNow(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);


        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExcludeNow(TEntity entity, IEnumerable<string> propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //EntityEntry<TEntity> InsertOrUpdateExcludeNow(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(TEntity entity, params string[] propertyNames);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propertyPredicates);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyNames">属性名</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(TEntity entity, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default);

        ///// <summary>
        ///// 新增或更新一条排除特定属性记录并立即提交
        ///// </summary>
        ///// <param name="entity">实体</param>
        ///// <param name="propertyPredicates">属性表达式</param>
        ///// <param name="cancellationToken">取消异步令牌</param>
        ///// <returns>数据库中的实体</returns>
        //Task<EntityEntry<TEntity>> InsertOrUpdateExcludeNowAsync(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> propertyPredicates, CancellationToken cancellationToken = default);
    }
}