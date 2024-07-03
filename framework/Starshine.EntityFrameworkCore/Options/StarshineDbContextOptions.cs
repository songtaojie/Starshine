// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Extensions;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 上下文配置
/// </summary>
public class StarshineDbContextOptions : IStarshineDbContextOptionsBuilder
{
    /// <summary>
    /// DbContext
    /// </summary>
    public Type OriginalDbContextType { get; }

    internal Dictionary<string, Type> DbContextReplacements { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="originalDbContextType"></param>
    /// <param name="services"></param>
    public StarshineDbContextOptions(Type originalDbContextType, IServiceCollection services)
    {
        OriginalDbContextType = originalDbContextType;
        Services = services;
        DbContextReplacements = new Dictionary<string, Type>();
    }
    /// <summary>
    /// 数据库提供商
    /// </summary>
    public EfCoreDatabaseProvider?  Provider { get; set; }

    /// <summary>
    /// 注册默认的仓储
    /// </summary>
    public bool RegisterDefaultRepositories { get; private set; }

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string? ConnectionString {  get; set; }

    /// <summary>
    /// 迁移类库名称
    /// </summary>
    public string? MigrationAssemblyName { get; set; }

    /// <summary>
    /// 数据库版本
    /// </summary>
    public object? Version {  get; set; }

    /// <summary>
    /// db配置
    /// </summary>
    internal Action<DbContextOptionsBuilder>? DbContextOptions { get; private set; }

    /// <summary>
    /// 服务
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextOptions"></param>
    public void Configure([NotNull] Action<DbContextOptionsBuilder> dbContextOptions)
    {
        DbContextOptions = dbContextOptions;
    }


    internal Type GetReplacedTypeOrSelf(Type dbContextType)
    {
        var replacementType = dbContextType;
        while (true)
        {
            var foundType = DbContextReplacements.LastOrDefault(x => x.Key == dbContextType.FullName);
            if (!foundType.Equals(default(KeyValuePair<string, Type>)))
            {
                if (foundType.Value == dbContextType)
                {
                    throw new Exception(
                        "Circular DbContext replacement found for " +
                        dbContextType.AssemblyQualifiedName
                    );
                }
                replacementType = foundType.Value;
            }
            else
            {
                return replacementType;
            }
        }
    }

    /// <summary>
    /// 获取hashcode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        if (Provider != null) hashCode.Add(Provider);
        if(!string.IsNullOrEmpty(ConnectionString)) hashCode.Add(ConnectionString);
        if(Version!=null) hashCode.Add(Version);
        //hashCode.Add(this.DbContextOptions.Options.GetHashCode());
        return hashCode.ToHashCode();
    }

    /// <summary>
    /// 添加默认的
    /// </summary>
    /// <returns></returns>
    public IStarshineDbContextOptionsBuilder AddDefaultRepositories()
    {
        var repositoryType = typeof(IRepository);
        var effectiveTypes = StarshineApp.EffectiveTypes.Where(t => t.IsPublic && !t.IsAbstract && (repositoryType.IsAssignableFrom(t)));
        var eFCoreRepositoryType = typeof(EFCoreRepository<,>);
        var readOnlyRepositoryType = typeof(ReadOnlyRepository<>);
        foreach (var type in effectiveTypes)
        {
            Type? entityType = null;
            if (eFCoreRepositoryType.IsAssignableFromGenericType(type, out Type selectType))
            {
                var s1 = selectType.GenericTypeArguments;
                var s2 = selectType.GetGenericArguments();
                entityType = selectType.GetGenericTypeDefinition();
                var efCoreRepositoryInterface = typeof(IEFCoreRepository<,>).MakeGenericType(OriginalDbContextType, entityType);
                //Services.TryAddScoped(efCoreRepositoryInterface, type);
            }

            if (eFCoreRepositoryType.IsAssignableFromGenericType(type))
            {
                var s = type.GetGenericParameterConstraints();
                //entityType = type.GetGenericTypeDefinition()[1];
                //var efCoreRepositoryInterface = typeof(IEFCoreRepository<,>).MakeGenericType(OriginalDbContextType, entityType);
                //Services.TryAddScoped(efCoreRepositoryInterface, type);
            }
            if (readOnlyRepositoryType.IsAssignableFromGenericType(type))
            {
                entityType ??= type.GenericTypeArguments.First();
                var readOnlyRepositoryInterface = typeof(IReadOnlyRepository<>).MakeGenericType(entityType);
                Services.TryAddScoped(readOnlyRepositoryInterface, type);
            }

        }
        return this;
    }
}
