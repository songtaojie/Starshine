using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Common;
using System.Diagnostics.CodeAnalysis;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class EFCoreRepository<TDbContext, TEntity> : OperableRepository<TEntity>, IEFCoreRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity, new()
    {

        /// <summary>
        /// 数据库上下文池
        /// </summary>
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scoped">服务提供器</param>
        public EFCoreRepository(IServiceProvider scoped) : base(scoped)
        {
            // 初始化服务提供器
            _dbContextProvider = scoped.GetRequiredService<IDbContextProvider<TDbContext>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<TDbContext> GetDbContextAsync()
        {
            return _dbContextProvider.GetDbContextAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DbSet<TEntity>> GetDbSetAsync()
        {
            return (await GetDbContextAsync()).Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<EntityEntry<TEntity>> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var savedEntity = await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return savedEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async override Task<TEntity?> FindOrDefaultAsync(object key, CancellationToken cancellationToken = default)
        {
            var entities = await GetDbSetAsync();
            return entities.Find(new object[] { key });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracking"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        public async override Task<IQueryable<TEntity>> GetQueryableAsync(bool? tracking = null, bool ignoreQueryFilters = false)
        {
            var query = (await GetDbSetAsync()).AsQueryable();
            if(tracking == true) query = query.AsNoTracking();
            if(ignoreQueryFilters) query = query.IgnoreQueryFilters();
            return query;
        }

        async Task<DbContext> IEFCoreRepository<TEntity>.GetDbContextAsync()
        {
            return await GetDbContextAsync();
        }
    }
}