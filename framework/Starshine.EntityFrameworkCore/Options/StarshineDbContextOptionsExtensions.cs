// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Starshine.EntityFrameworkCore;
internal static class StarshineDbContextOptionsExtensions
{
    internal static DbContextOptionsBuilder UseDatabase<TContext>(this DbContextOptionsBuilder optionsBuilder, IStarshineDbContextOptionsBuilder contextOptions)
        where TContext : DbContext
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbContextOptionsBuilder = optionsBuilder;

            // 获取数据库上下文特性
            var dbContextAttribute = DatabaseProviderHelper.GetStarshineDbContextAttribute(typeof(TContext));
            contextOptions.Provider ??= dbContextAttribute?.Provider;
            if (!string.IsNullOrWhiteSpace(contextOptions.ConnectionString) && contextOptions.Provider != null)
            {
                var useMethod = DatabaseProviderHelper.GetDatabaseProviderUseMethod(contextOptions.Provider.Value);// 调用对应数据库程序集
                if (useMethod != null)
                {
                    var optionsAction = GetRelationalDbContextOptionsAction(contextOptions);
                    // 处理最新第三方 MySql 包兼容问题
                    if (contextOptions.Provider == EFCoreDatabaseProvider.MySql)
                    {
                        var mySqlVersion = contextOptions.Version ?? DatabaseProviderHelper.GetMySqlVersion(contextOptions.ConnectionString);
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object?[] { optionsBuilder, contextOptions.ConnectionString, mySqlVersion, optionsAction }) as DbContextOptionsBuilder;
                    }
                    // 处理 Oracle 11 兼容问题
                    else if (contextOptions.Provider == EFCoreDatabaseProvider.Oracle)
                    {
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object[] { optionsBuilder, contextOptions.ConnectionString, optionsAction }) as DbContextOptionsBuilder;
                    }
                    else
                    {
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object[] { optionsBuilder, contextOptions.ConnectionString, optionsAction }) as DbContextOptionsBuilder;
                    }
                }


            }

            // 解决分表分库
            if (dbContextAttribute?.Mode == DbContextMode.Dynamic) optionsBuilder
                  .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
            
        }
        return optionsBuilder;
    }

    

    /// <summary>
    /// 静态构造方法
    /// </summary>
    static StarshineDbContextOptionsExtensions()
    {
        RelationalDbContextOptionsActions = new ();
    }
    /// <summary>
    /// 配置Code First 程序集 Action委托
    /// </summary>
    private static readonly ConcurrentDictionary<int, Action<IRelationalDbContextOptionsBuilderInfrastructure>>  RelationalDbContextOptionsActions;
    
    /// <summary>
    /// 获取设置
    /// </summary>
    /// <param name="contextOptions"></param>
    /// <returns></returns>
    private static Action<IRelationalDbContextOptionsBuilderInfrastructure> GetRelationalDbContextOptionsAction(IStarshineDbContextOptionsBuilder contextOptions)
    {
        if (contextOptions.Provider == EFCoreDatabaseProvider.Oracle)
        {
            return RelationalDbContextOptionsActions.GetOrAdd(contextOptions.GetHashCode(), options =>
            {
                var optionsType = options.GetType();
                if (contextOptions.Version != null)
                {
                    // 处理版本号
                    optionsType.GetMethod("UseOracleSQLCompatibility")?.Invoke(options, new[] { contextOptions.Version });
                }

                // 处理迁移程序集
                if (!string.IsNullOrEmpty(contextOptions.MigrationAssemblyName))
                {
                    optionsType.GetMethod("MigrationsAssembly")?.Invoke(options, new[] { contextOptions.MigrationAssemblyName });
                }
            });
        }
        else
        {
            return RelationalDbContextOptionsActions.GetOrAdd(contextOptions.GetHashCode(), options =>
            {
                if (!string.IsNullOrEmpty(contextOptions.MigrationAssemblyName))
                {
                    options.GetType().GetMethod("MigrationsAssembly")
                        ?.Invoke(options, new[] { contextOptions.MigrationAssemblyName });
                }

            });
        }
    }
}
