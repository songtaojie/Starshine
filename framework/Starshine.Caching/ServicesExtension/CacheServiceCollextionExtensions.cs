using FreeRedis;
using Starshine.Caching;
using Starshine.Caching.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存的扩展类
    /// </summary>
    public static class CacheServiceCollextionExtensions
    {
        private const string CacheSettingsKey = "CacheSettings";

        /// <summary>
        /// 增加了一个默认的实现<see cref="ICache"/>，将项目存储在 memory/redis到<see cref="IServiceCollection" />。
        /// 需要分布式缓存工作的框架可以安全地将此依赖项添加到其依赖项列表中，以确保至少有一个实现可用。
        /// </summary>
        /// <remarks>
        /// <see cref="AddCache(IServiceCollection)"/> 内存缓存应该只在单服务器场景中使用，因为该缓存将项目存储在内存中，
        /// 不会跨多台机器扩展。对于这些场景，建议使用合适的分布式缓存(redis缓存)，可以跨多台机器扩展。
        /// </remarks>
        /// <param name="services"> <see cref="IServiceCollection" /> 添加服务.</param>
        /// <returns> <see cref="IServiceCollection"/> 这样额外的调用就可以被链接起来。</returns>
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            return AddCacheCore(services);
        }

        /// <summary>
        /// 增加了一个默认的实现<see cref="ICache"/>，将项目存储在 memory/redis到<see cref="IServiceCollection" />。
        /// 需要分布式缓存工作的框架可以安全地将此依赖项添加到其依赖项列表中，以确保至少有一个实现可用。
        /// </summary>
        /// <remarks>
        /// <see cref="AddCache(IServiceCollection)"/> 内存缓存应该只在单服务器场景中使用，因为该缓存将项目存储在内存中，
        /// 不会跨多台机器扩展。对于这些场景，建议使用合适的分布式缓存(redis缓存)，可以跨多台机器扩展。
        /// </remarks>
        /// <param name="services"> <see cref="IServiceCollection" /> 添加服务.</param>
        /// <param name="setupAction">
        ///  <see cref="Action{CacheSettingsOptions}"/> 要提供的配置 <see cref="CacheSettingsOptions"/>.
        /// </param>
        /// <returns> <see cref="IServiceCollection"/> 这样额外的调用就可以被链接起来。</returns>
        public static IServiceCollection AddCache(this IServiceCollection services, Action<CacheSettingsOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
            return AddCacheCore(services, setupAction);
        }


        private static IServiceCollection AddCacheCore(IServiceCollection services, Action<CacheSettingsOptions>? setupAction = null)
        {
            services.AddOptions<CacheSettingsOptions>()
               .BindConfiguration(CacheSettingsKey)
               .Configure(options =>
               {
                   setupAction?.Invoke(options);
               });
            services.AddOptions<MemoryCacheOptions>()
              .Configure<IOptions<CacheSettingsOptions>>((options, cacheSettingsOptions) =>
              {
                  var cacheSettings = cacheSettingsOptions.Value;
                  if (cacheSettings.Memory != null)
                  {
                      if (cacheSettings.Memory.CompactionPercentage > 0 && cacheSettings.Memory.CompactionPercentage < 1)
                      {
                          options.CompactionPercentage = cacheSettings.Memory.CompactionPercentage;
                      }
                      if (cacheSettings.Memory.SizeLimit.HasValue)
                      {
                          options.SizeLimit = cacheSettings.Memory.SizeLimit;
                      }
                  }
              });
            services.AddOptions<MemoryDistributedCacheOptions>()
               .Configure<IOptions<CacheSettingsOptions>>((options, cacheSettingsOptions) =>
               {
                   var cacheSettings = cacheSettingsOptions.Value;
                   if (cacheSettings.Memory != null)
                   {
                       if (cacheSettings.Memory.CompactionPercentage > 0 && cacheSettings.Memory.CompactionPercentage < 1)
                       {
                           options.CompactionPercentage = cacheSettings.Memory.CompactionPercentage;
                       }
                       if (cacheSettings.Memory.SizeLimit.HasValue)
                       {
                           options.SizeLimit = cacheSettings.Memory.SizeLimit;
                       }
                   }
               });
            services.AddMemoryCache();
            services.TryAddSingleton<IDistributedCache>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                if (options.CacheType == CacheTypeEnum.Redis)
                {
                    var redisClient = sp.GetService<IRedisClient>();
                    return new DistributedCache(redisClient as RedisClient);
                }
                else
                {
                    return new MemoryDistributedCache(sp.GetService<IOptions<MemoryDistributedCacheOptions>>(), sp.GetService<ILoggerFactory>());
                }
            });

            services.TryAddSingleton<IRedisClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                if (options.CacheType == CacheTypeEnum.Redis)
                {
                    var redisOptions = options.Redis;
                    if (redisOptions == null || string.IsNullOrEmpty(redisOptions.ConnectionString))
                        throw new ArgumentNullException(nameof(RedisCacheSettingsOptions.ConnectionString));

                    if (redisOptions.SlaveConnectionStrings == null || !redisOptions.SlaveConnectionStrings.Any())
                    {
                        return new RedisClient(redisOptions.ConnectionString);
                    }
                    else
                    {
                        var slaveConnectionStrings = redisOptions.SlaveConnectionStrings.Select(r => ConnectionStringBuilder.Parse(r)).ToArray();
                        return new RedisClient(redisOptions.ConnectionString, slaveConnectionStrings);
                    }
                }
                else
                {
                    throw new NotImplementedException(nameof(IRedisClient));
                }
            });
            services.TryAddSingleton<ICache>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<CacheSettingsOptions>>().Value;
                if (options.CacheType == CacheTypeEnum.Redis)
                {
                    return new RedisCache(sp.GetRequiredService<IRedisClient>());
                }
                else
                {
                    return new DefaultCache(sp.GetRequiredService<IMemoryCache>());
                }
            });
            return services;
        }
    }
}
