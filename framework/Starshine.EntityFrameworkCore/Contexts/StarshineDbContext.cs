using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 默认应用数据库上下文
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    public abstract class StarshineDbContext<TDbContext> : StarshineDbContext<TDbContext, DefaultDbContextProvider>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public StarshineDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }
    }

    /// <summary>
    /// 应用数据库上下文
    /// </summary>
    /// <typeparam name="TDbContext">数据库上下文</typeparam>
    /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
    public abstract class StarshineDbContext<TDbContext, TDbContextLocator> : DbContext
        where TDbContext : DbContext
        where TDbContextLocator : class, IDbContextLocator
    {
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
            // 配置数据库上下文实体
            AppDbContextBuilder.ConfigureDbContextEntity(modelBuilder, this, typeof(TDbContextLocator));
        }

        /// <summary>
        /// 获取租户信息
        /// </summary>
        public virtual Tenant Tenant
        {
            get
            {
                // 如果没有实现多租户方式，则无需查询
                if (Db.CustomizeMultiTenants || !typeof(IPrivateMultiTenant).IsAssignableFrom(GetType())) return default;

                // 判断 HttpContext 是否存在
                var httpContext = App.HttpContext;
                if (httpContext == null) return default;

                // 获取主机地址
                var host = httpContext.Request.Host.Value;

                // 获取服务提供器
                var serviceProvider = httpContext.RequestServices;

                // 从分布式缓存中读取或查询数据库
                var tenantCachedKey = $"MULTI_TENANT:{host}";
                var distributedCache = serviceProvider.GetService<IDistributedCache>();
                var cachedValue = distributedCache?.GetString(tenantCachedKey);

                // 当前租户
                Tenant currentTenant;

                // 获取序列化库
                var jsonSerializerProvider = serviceProvider.GetService<IJsonSerializerProvider>();

                // 如果 Key 不存在
                if (string.IsNullOrWhiteSpace(cachedValue))
                {
                    // 解析租户上下文
                    var dbContextResolve = serviceProvider.GetService<Func<Type, IScoped, DbContext>>();
                    if (dbContextResolve == null) return default;

                    var tenantDbContext = dbContextResolve(typeof(MultiTenantDbContextLocator), default);
                    ((dynamic)tenantDbContext).UseUnitOfWork = false;   // 无需载入事务

                    currentTenant = tenantDbContext.Set<Tenant>().AsNoTracking().FirstOrDefault(u => u.Host == host);
                    if (currentTenant != null)
                    {
                        distributedCache?.SetString(tenantCachedKey, jsonSerializerProvider.Serialize(currentTenant));
                    }
                }
                else currentTenant = jsonSerializerProvider.Deserialize<Tenant>(cachedValue);

                return currentTenant;
            }
        }

    }
}