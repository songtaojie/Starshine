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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="originalDbContextType"></param>
    /// <param name="services"></param>
    public StarshineDbContextOptions(Type originalDbContextType, IServiceCollection services)
    {
        OriginalDbContextType = originalDbContextType;
        Services = services;
    }
    /// <summary>
    /// 数据库提供商
    /// </summary>
    public EFCoreDatabaseProvider?  Provider { get; set; }

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
        var operableRepositoryType = typeof(OperableRepository<>);
        var readOnlyRepositoryType = typeof(ReadOnlyRepository<>);
        foreach (var type in effectiveTypes)
        {
            Type? entityType = null;
            if (eFCoreRepositoryType.IsAssignableFromGenericType(type, out Type selectType))
            {
                var genericArguments = selectType.GetGenericArguments();
                entityType = genericArguments[1];
                var efCoreRepositoryInterface = typeof(IEFCoreRepository<>).MakeGenericType(entityType);
                Services.TryAddScoped(efCoreRepositoryInterface, type);
            }

            if (operableRepositoryType.IsAssignableFromGenericType(type, out selectType))
            {
                entityType ??= selectType.GetGenericArguments().First();
                var readOnlyRepositoryInterface = typeof(IOperableRepository<>).MakeGenericType(entityType);
                Services.TryAddScoped(readOnlyRepositoryInterface, type);
            }

            if (readOnlyRepositoryType.IsAssignableFromGenericType(type, out selectType))
            {
                entityType ??= selectType.GetGenericArguments().First();
                var readOnlyRepositoryInterface = typeof(IReadOnlyRepository<>).MakeGenericType(entityType);
                Services.TryAddScoped(readOnlyRepositoryInterface, type);
            }

        }
        return this;
    }
}
