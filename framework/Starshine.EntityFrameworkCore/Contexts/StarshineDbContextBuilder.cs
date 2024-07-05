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
    ///// <summary>
    ///// 数据库实体相关类型
    ///// </summary>
    //private static readonly List<Type> EntityTypes;
    private static readonly string DefaultDbContextId = Guid.NewGuid().ToString();
    /// <summary>
    /// ModelBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string, List<Type>> EntityTypes;

    /// <summary>
    /// ModelBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string,Type> ModelBuilderFilters;

    /// <summary>
    /// EntityTypeBuilder实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> EFCoreEntitySeedDatas;

    /// <summary>
    /// EntityTypeConfiguration实现类
    /// </summary>
    private static readonly ConcurrentDictionary<string, Type> EntityTypeConfigurations;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    static StarshineDbContextBuilder()
    {
        EntityTypes = new();
        ModelBuilderFilters = new();
        EFCoreEntitySeedDatas = new();
        EntityTypeConfigurations = new();
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
        if (EntityTypeConfigurations.Any())
        {
            return EntityTypeConfigurations.GetValueOrDefault(entityType.FullName ?? entityType.Name);
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

    internal static Type? GetEntityTypeConfiguration(Type entityType)
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
        var iEFCoreEntitySeedDataType = typeof(IEFCoreEntitySeedData<>);
        var iEntityType = typeof(IEntity);
        var iEntityTypeConfigurationType = typeof(IEntityTypeConfiguration<>);
        var iEntityContextMarkerType = typeof(IEntityContextMarker<>);
        foreach (var type in StarshineApp.EffectiveTypes)
        {
            if (type.IsAbstract || type.IsInterface) continue;
            if (iEntityType.IsAssignableFrom(type))
            {
                string key = DefaultDbContextId;
                if (iEntityContextMarkerType.IsAssignableFromGenericType(type, out Type dbContextType))
                {
                    key = GetGenericTypeName(dbContextType);
                }
                if (EntityTypes.ContainsKey(key))
                {
                    var entityList = EntityTypes.GetValueOrDefault(key);
                    if (entityList != null && !entityList.Contains(type))
                    {
                        entityList.Add(type);
                        EntityTypes[key] = entityList;
                    }
                }
                else
                {
                    EntityTypes.TryAdd(key, new List<Type> { type });
                }
            }
            if (iModelBuilderFilterType.IsAssignableFromGenericType(type,out Type genericType))
            {
                ModelBuilderFilters.TryAdd(GetGenericTypeName(genericType), type);
            }
           
            if (iEFCoreEntitySeedDataType.IsAssignableFromGenericType(type, out Type genericType2))
            {
                EFCoreEntitySeedDatas.TryAdd(GetGenericTypeName(genericType2), type);
            }
            if (iEntityTypeConfigurationType.IsAssignableFromGenericType(type, out Type genericType3))
            {
                EntityTypeConfigurations.TryAdd(GetGenericTypeName(genericType3), type);
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
