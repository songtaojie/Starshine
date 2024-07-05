// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Starshine.Common;
using Starshine.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
internal static class StarshineDbContextBuilder
{
    /// <summary>
    /// 数据库实体相关类型
    /// </summary>
    private static readonly List<Type> EntityTypes;

    /// <summary>
    /// ModelBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string,Type> ModelBuilderFilters;

    /// <summary>
    /// EntityTypeBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> EntityTypeBuilders;

    /// <summary>
    /// EntityTypeBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> EFCoreEntitySeedDatas;

    /// <summary>
    /// 构造函数
    /// </summary>
    static StarshineDbContextBuilder()
    {
        EntityTypes = new List<Type>();
        ModelBuilderFilters = new ConcurrentDictionary<string, Type>();
        EntityTypeBuilders = new ConcurrentDictionary<string, Type>();
        EFCoreEntitySeedDatas = new ConcurrentDictionary<string, Type>();
        Init();
    }

    internal static Type? GetModelBuilderFilterType(Type dbContextType)
    {
        if (ModelBuilderFilters.Any())
        {
            return ModelBuilderFilters.GetValueOrDefault(dbContextType.FullName ?? dbContextType.Name);
        }
        return null;
    }

    internal static Type? GetEntityTypeBuilderType(Type entityType)
    {
        if (EntityTypeBuilders.Any())
        {
            return EntityTypeBuilders.GetValueOrDefault(entityType.FullName ?? entityType.Name);
        }
        return null;
    }

    internal static Type? GetEFCoreEntitySeedDataType(Type entityType)
    {
        if (EFCoreEntitySeedDatas.Any())
        {
            return EFCoreEntitySeedDatas.GetValueOrDefault(entityType.FullName ?? entityType.Name);
        }
        return null;
    }

    private static void Init()
    {
        var iModelBuilderFilterType = typeof(IModelBuilderFilter<>);
        var iEntityTypeBuilderType = typeof(IEntityTypeBuilder<>);
        var iEFCoreEntitySeedDataType = typeof(IEFCoreEntitySeedData<>);
        var iEntityType = typeof(IEntity);
        foreach (var type in StarshineApp.EffectiveTypes)
        {
            if (type.IsAbstract || type.IsInterface) continue;
            if (iEntityType.IsAssignableFrom(type))
            {
                EntityTypes.Add(type);
            }
            if (iModelBuilderFilterType.IsAssignableFromGenericType(type,out Type genericType))
            {
                ModelBuilderFilters.TryAdd(GetGenericTypeName(genericType), type);
            }
            if (iEntityTypeBuilderType.IsAssignableFromGenericType(type, out Type genericType1))
            {
                EntityTypeBuilders.TryAdd(GetGenericTypeName(genericType1), type);
            }
            if (iEFCoreEntitySeedDataType.IsAssignableFromGenericType(type, out Type genericType2))
            {
                EFCoreEntitySeedDatas.TryAdd(GetGenericTypeName(genericType2), type);
            }
        }
    }

    private static string GetGenericTypeName(Type genericType)
    {
        if (genericType.IsGenericType)
        {
            var genericArguments = genericType.GetGenericArguments();
            if(genericArguments.Any())return genericArguments.First().FullName ?? genericArguments.First().Name;
        }
        return genericType.FullName ?? genericType.Name;
    }
}
