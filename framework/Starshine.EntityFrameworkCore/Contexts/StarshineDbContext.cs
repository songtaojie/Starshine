using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Starshine.EntityFrameworkCore.Extensions;
using Starshine.EntityFrameworkCore.Modeling;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 应用数据库上下文
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    public abstract class StarshineDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {

        private static readonly MethodInfo ConfigureEntityTypeBuilderMethodInfo
          = typeof(StarshineDbContext<TDbContext>)
              .GetMethod(nameof(ConfigureEntityTypeBuilder),BindingFlags.Instance | BindingFlags.NonPublic)!;

        /// <summary>
        /// 保存失败回滚
        /// </summary>
        public virtual bool FailedRollback { get; protected set; } = true;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public StarshineDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 启用实体跟踪（默认值）
        /// </summary>
        public virtual bool EnabledEntityStateTracked { get; protected set; } = true;

        /// <summary>
        /// 数据库上下文初始化调用方法
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 数据库上下文配置模型调用方法
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var modelBuilderFilter = this.GetService<IModelBuilderFilter<TDbContext>>();
            if (modelBuilderFilter != null)
            {
                modelBuilderFilter.OnModelCreating(modelBuilder, this);
            }
            TrySetDatabaseProvider(modelBuilder);
            // 初始化所有类型
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureEntityTypeBuilderMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
            if (modelBuilderFilter != null)
            {
                modelBuilderFilter.OnOnModelCreated(modelBuilder, this);
            }
        }

        ///// <summary>
        ///// 获取租户信息
        ///// </summary>
        //public virtual Tenant Tenant
        //{
        //    get
        //    {
        //        // 如果没有实现多租户方式，则无需查询
        //        if (Db.CustomizeMultiTenants || !typeof(IPrivateMultiTenant).IsAssignableFrom(GetType())) return default;

        //        // 判断 HttpContext 是否存在
        //        var httpContext = App.HttpContext;
        //        if (httpContext == null) return default;

        //        // 获取主机地址
        //        var host = httpContext.Request.Host.Value;

        //        // 获取服务提供器
        //        var serviceProvider = httpContext.RequestServices;

        //        // 从分布式缓存中读取或查询数据库
        //        var tenantCachedKey = $"MULTI_TENANT:{host}";
        //        var distributedCache = serviceProvider.GetService<IDistributedCache>();
        //        var cachedValue = distributedCache?.GetString(tenantCachedKey);

        //        // 当前租户
        //        Tenant currentTenant;

        //        // 获取序列化库
        //        var jsonSerializerProvider = serviceProvider.GetService<IJsonSerializerProvider>();

        //        // 如果 Key 不存在
        //        if (string.IsNullOrWhiteSpace(cachedValue))
        //        {
        //            // 解析租户上下文
        //            var dbContextResolve = serviceProvider.GetService<Func<Type, IScoped, DbContext>>();
        //            if (dbContextResolve == null) return default;

        //            var tenantDbContext = dbContextResolve(typeof(MultiTenantDbContextLocator), default);
        //            ((dynamic)tenantDbContext).UseUnitOfWork = false;   // 无需载入事务

        //            currentTenant = tenantDbContext.Set<Tenant>().AsNoTracking().FirstOrDefault(u => u.Host == host);
        //            if (currentTenant != null)
        //            {
        //                distributedCache?.SetString(tenantCachedKey, jsonSerializerProvider.Serialize(currentTenant));
        //            }
        //        }
        //        else currentTenant = jsonSerializerProvider.Deserialize<Tenant>(cachedValue);

        //        return currentTenant;
        //    }
        //}

        /// <summary>
        /// 设置DatabaseProvider
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void TrySetDatabaseProvider(ModelBuilder modelBuilder)
        {
            var provider = GetDatabaseProvider(modelBuilder);
            if (provider != null)
            {
                modelBuilder.SetDatabaseProvider(provider.Value);
            }
        }

        /// <summary>
        /// 获取DatabaseProvider
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        protected virtual EfCoreDatabaseProvider? GetDatabaseProvider(ModelBuilder modelBuilder)
        {
            switch (Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    return EfCoreDatabaseProvider.SqlServer;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return EfCoreDatabaseProvider.PostgreSQL;
                case "Pomelo.EntityFrameworkCore.MySql":
                    return EfCoreDatabaseProvider.MySql;
                case "Oracle.EntityFrameworkCore":
                case "Devart.Data.Oracle.Entity.EFCore":
                    return EfCoreDatabaseProvider.Oracle;
                case "Microsoft.EntityFrameworkCore.Sqlite":
                    return EfCoreDatabaseProvider.Sqlite;
                case "Microsoft.EntityFrameworkCore.InMemory":
                    return EfCoreDatabaseProvider.InMemory;
                case "FirebirdSql.EntityFrameworkCore.Firebird":
                    return EfCoreDatabaseProvider.Firebird;
                case "Microsoft.EntityFrameworkCore.Cosmos":
                    return EfCoreDatabaseProvider.Cosmos;
                default:
                    return null;
            }
        }


        /// <summary>
        /// 配置数据库实体类型构建器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="mutableEntityType">实体类型</param>
        protected virtual void ConfigureEntityTypeBuilder<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }
            var entityType = mutableEntityType.ClrType;
            if (!typeof(IEntity).IsAssignableFrom(entityType))
            {
                return;
            }
            modelBuilder.Entity<TEntity>().ConfigureByConvention();

            // 获取泛型服务的类型  
            Type builderType = typeof(IEntityTypeBuilder<>).MakeGenericType(entityType);

            //配置数据库实体种子数据
            // 解析服务  
            var efCoreEntitySeedData = this.GetService<IEfCoreEntitySeedData<TEntity>>();
            // 检查是否解析到了服务  
            if (efCoreEntitySeedData != null)
            {
                var seedData = efCoreEntitySeedData.HasData(this);
                if (seedData != null && seedData.Any())
                {
                    modelBuilder.Entity<TEntity>().HasData(seedData);
                }
            }

            // 解析服务  
            var entityTypeBuilder = this.GetService<IEntityTypeBuilder<TEntity>>();
            // 检查是否解析到了服务  
            if (entityTypeBuilder != null)
            {
                entityTypeBuilder.Configure(modelBuilder.Entity<TEntity>(), mutableEntityType);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="options"></param>
        public virtual void Initialize(UnitOfWorkOptions options)
        {
            if (options.Timeout.HasValue &&
                Database.IsRelational() &&
                !Database.GetCommandTimeout().HasValue)
            {
                Database.SetCommandTimeout(TimeSpan.FromMilliseconds(options.Timeout.Value));
            }

            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;

            //ChangeTracker.Tracked += ChangeTracker_Tracked;
            //ChangeTracker.StateChanged += ChangeTracker_StateChanged;
            
        }

        ///// <summary>
        ///// 配置全局过滤
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="modelBuilder"></param>
        ///// <param name="mutableEntityType"></param>
        //protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        //    where TEntity : class
        //{
        //    if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
        //    {
        //        var filterExpression = CreateFilterExpression<TEntity>();
        //        if (filterExpression != null)
        //        {
        //            modelBuilder.Entity<TEntity>().HasAbpQueryFilter(filterExpression);
        //        }
        //    }
        //}


        //protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        //{
        //    if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        //    {
        //        return true;
        //    }

        //    if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        //where TEntity : class
        //{
        //    Expression<Func<TEntity, bool>>? expression = null;

        //    if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        //    {
        //        expression = e => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(e, "IsDeleted");
        //    }

        //    if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
        //    {
        //        Expression<Func<TEntity, bool>> multiTenantFilter = e => !IsMultiTenantFilterEnabled || EF.Property<Guid>(e, "TenantId") == CurrentTenantId;
        //        expression = expression == null ? multiTenantFilter : QueryFilterExpressionHelper.CombineExpressions(expression, multiTenantFilter);
        //    }

        //    return expression;
        //}
    }
}