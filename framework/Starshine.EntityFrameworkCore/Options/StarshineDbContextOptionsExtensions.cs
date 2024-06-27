// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Starshine.EntityFrameworkCore.Internal;
using StackExchange.Profiling.Internal;
using System.Collections.Concurrent;
using System.Reflection;

namespace Starshine.EntityFrameworkCore;
internal static class StarshineDbContextOptionsExtensions
{
    internal static DbContextOptionsBuilder UseDatabase<TContext>(this DbContextOptionsBuilder optionsBuilder, StarshineDbContextOptions contextOptions)
        where TContext : DbContext
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbContextOptionsBuilder = optionsBuilder;

            // 获取数据库上下文特性
            var dbContextAttribute = DbProvider.GetAppDbContextAttribute(typeof(TContext));
            if (!string.IsNullOrWhiteSpace(contextOptions.ConnectionString) && contextOptions.Provider != null)
            {
                contextOptions.Provider ??= dbContextAttribute?.Provider;
                if(contextOptions.Provider == null)return optionsBuilder;
                var useMethod = DbProviderHelper.GetDatabaseProviderUseMethod(contextOptions.Provider.Value);// 调用对应数据库程序集
                if (useMethod != null)
                {
                    var optionsAction = GetRelationalDbContextOptionsAction(contextOptions);
                    // 处理最新第三方 MySql 包兼容问题
                    if (contextOptions.Provider == EfCoreDatabaseProvider.MySql)
                    {
                        var mySqlVersion = contextOptions.Version ?? DbProviderHelper.GetMySqlVersion(contextOptions.ConnectionString);
                        dbContextOptionsBuilder = useMethod
                            .Invoke(null, new object?[] { optionsBuilder, contextOptions.ConnectionString, mySqlVersion, optionsAction }) as DbContextOptionsBuilder;
                    }
                    // 处理 Oracle 11 兼容问题
                    else if (contextOptions.Provider == EfCoreDatabaseProvider.Oracle)
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
    private static Action<IRelationalDbContextOptionsBuilderInfrastructure> GetRelationalDbContextOptionsAction(StarshineDbContextOptions contextOptions)
    {
        if (contextOptions.Provider == EfCoreDatabaseProvider.Oracle)
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
                    options.GetType().GetMethod("MigrationsAssembly")?.Invoke(options, new[] { contextOptions.MigrationAssemblyName });
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
