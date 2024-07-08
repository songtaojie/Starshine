// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// 模型构建器筛选器实例
    /// </summary>
    private static Dictionary<string,Type> EntityMutableTableTypes { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    static StarshineDbContextBuilder()
    {
        EntityTypes = new();
        ModelBuilderFilters = new();
        EFCoreEntitySeedDatas = new();
        EntityTypeConfigurations = new();
        EntityMutableTableTypes = new();
        Init();
    }

    internal static IEnumerable<Type>? GetEntityTypes(Type dbContextType)
    {
        var key = dbContextType.FullName ?? dbContextType.Name;
        if (EntityTypes.ContainsKey(key))
        {
            return EntityTypes.GetValueOrDefault(key);
        }
        return EntityTypes.GetValueOrDefault(DefaultDbContextId);
    }

    internal static IEntityTypeConfiguration<TEntity>? GetEntityType<TEntity>()
        where TEntity : class
    {
        if (EntityTypeConfigurations.Any())
        {
            var entityTypeConfiguration = EntityTypeConfigurations.GetValueOrDefault(GetGenericTypeName(typeof(TEntity)));
            if (entityTypeConfiguration != null)
            { 
                return Activator.CreateInstance(entityTypeConfiguration) as IEntityTypeConfiguration<TEntity>;
            }
        }

        return null;
    }

    internal static Type? GetModelBuilderFilterType(Type dbContextType)
    {
        if (ModelBuilderFilters.Any())
        {
            return ModelBuilderFilters.GetValueOrDefault(GetGenericTypeName(dbContextType));
        }
        return null;
    }

    internal static Type? GetEntityTypeBuilderType(Type entityType)
    {
        if (EntityTypeConfigurations.Any())
        {
            return EntityTypeConfigurations.GetValueOrDefault(GetGenericTypeName(entityType));
        }
        return null;
    }

    internal static Type? GetEFCoreEntitySeedDataType(Type entityType)
    {
        if (EFCoreEntitySeedDatas.Any())
        {
            return EFCoreEntitySeedDatas.GetValueOrDefault(GetGenericTypeName(entityType));
        }
        return null;
    }

    internal static Type? GetEntityTypeConfiguration(Type entityType)
    {
        if (EFCoreEntitySeedDatas.Any())
        {
            return EFCoreEntitySeedDatas.GetValueOrDefault(GetGenericTypeName(entityType));
        }
        return null;
    }

    internal static Type? GetEntityMutableTableType(Type entityType)
    {
        if (EntityMutableTableTypes.Any())
        {
            return EntityMutableTableTypes.GetValueOrDefault(GetGenericTypeName(entityType));
        }
        return null;
    }

    private static void Init()
    {
        var iModelBuilderFilterType = typeof(IModelBuilderFilter<>);
        var iEFCoreEntitySeedDataType = typeof(IEFCoreEntitySeedData<>);
        var iEntityType = typeof(IEntity);
        var iEntityTypeConfigurationType = typeof(IEntityTypeConfiguration<>);
        var iEntityMutableTableType = typeof(IEntityMutableTable<>);
        foreach (var type in StarshineApp.EffectiveTypes)
        {
            if (!IsEffectiveType(type)) continue;
            if (iEntityType.IsAssignableFrom(type))
            {
                string key = DefaultDbContextId;
                var entityContextMarkerAttribute = type.GetCustomAttribute<EntityContextMarkerAttribute>(true);
                if (entityContextMarkerAttribute != null)
                {
                    key = GetGenericTypeName(entityContextMarkerAttribute.DbContextType);
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
            if (iEntityMutableTableType.IsAssignableFromGenericType(type, out Type genericType4))
            {
                EntityMutableTableTypes.Add(GetGenericTypeName(genericType4), type);
            }
        }
    }

    private static bool IsEffectiveType(Type type) 
    {
        if (type.IsAbstract || type.IsInterface || !type.IsClass || type.IsNotPublic)return false;
        var interfaces = type.GetInterfaces();
        if(!interfaces.Any()) return false;
        var iModelBuilderFilterType = typeof(IModelBuilderFilter<>);
        var iEFCoreEntitySeedDataType = typeof(IEFCoreEntitySeedData<>);
        var iEntityType = typeof(IEntity);
        var iEntityTypeConfigurationType = typeof(IEntityTypeConfiguration<>);
        var iEntityMutableTableType = typeof(IEntityMutableTable<>);
        return interfaces.Any(r => r.IsGenericType(iModelBuilderFilterType) || r.IsGenericType(iEFCoreEntitySeedDataType)
            || iEntityType.IsAssignableFrom(r) || r.IsGenericType(iEntityTypeConfigurationType)
            || r.IsGenericType(iEntityMutableTableType));
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
