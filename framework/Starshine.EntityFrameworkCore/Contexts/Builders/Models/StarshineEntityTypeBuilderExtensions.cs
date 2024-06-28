// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.EntityFrameworkCore.Extensions;
using Starshine.EntityFrameworkCore.Extensions.LinqBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Modeling;
/// <summary>
/// EntityTypeBuilder扩展
/// </summary>
public static class StarshineEntityTypeBuilderExtensions
{

    private static IEnumerable<Type> _entityMutableTableTypes;
    /// <summary>
    /// 模型构建器筛选器实例
    /// </summary>
    private static IEnumerable<Type> _modelBuilderFilters { get; set; }

    static StarshineEntityTypeBuilderExtensions()
    {
        _entityMutableTableTypes = App.EffectiveTypes.Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IEntityMutableTable<>))));
        _modelBuilderFilters = App.EffectiveTypes.Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IModelBuilderFilter<>))));

    }

    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureTableName();
        b.TryConfigureExtraProperties();
        b.TryConfigureObjectExtensions();
        b.TryConfigureMayHaveCreator();
        b.TryConfigureMustHaveCreator();
        b.TryConfigureSoftDelete();
        b.TryConfigureDeletionTime();
        b.TryConfigureDeletionAudited();
        b.TryConfigureCreationTime();
        b.TryConfigureLastModificationTime();
        b.TryConfigureModificationAudited();
        b.TryConfigureMultiTenant();
    }

    /// <summary>
    /// 配置表名
    /// </summary>
    /// <param name="entityBuilder"></param>
    /// <param name="dbContext"></param>
    public static void TryConfigureTableName(this EntityTypeBuilder entityBuilder, DbContext dbContext)
    {
        if (!entityBuilder.TryConfigureMutableTableName(dbContext))
        {
            entityBuilder.ConfigureTableName(dbContext);
        }
    }

    /// <summary>
    /// 配置实体动态表名
    /// </summary>
    /// <param name="entityBuilder">实体类型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    private static bool TryConfigureMutableTableName(this EntityTypeBuilder entityBuilder,DbContext dbContext)
    {
        var isSet = false;
        // 获取实体动态配置表配置
        var lastEntityMutableTableType = _entityMutableTableTypes.Where(t => t.GenericTypeArguments.Contains(entityBuilder.Metadata.ClrType)).LastOrDefault();
        if (lastEntityMutableTableType == null) return isSet;
        // 只应用于扫描的最后配置
        var instance = Activator.CreateInstance(lastEntityMutableTableType);
        var getTableNameMethod = lastEntityMutableTableType.GetMethod(nameof(IEntityMutableTable<object>.GetTableName));
        if(getTableNameMethod == null) return isSet;
        var tableName = getTableNameMethod.Invoke(instance, new object[] { dbContext }) as StarshineTableName;
        if (tableName != null && !string.IsNullOrEmpty(tableName.TableName))
        {
            // 设置动态表名
            entityBuilder.ToTable(tableName.TableName, tableName.Schema);
            isSet = true;
        }
        return isSet;
    }

    /// <summary>
    /// 配置实体表名
    /// </summary>
    /// <param name="entityBuilder">实体类型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    private static void ConfigureTableName(this EntityTypeBuilder entityBuilder,  DbContext dbContext)
    {
        var type = entityBuilder.Metadata.ClrType;
        // 获取表是否定义 [Table] 特性
        var tableAttribute = type.IsDefined(typeof(TableAttribute), true) 
            ? entityBuilder.Metadata.ClrType.GetCustomAttribute<TableAttribute>(true) 
            : default;

        // 排除无键实体或已经贴了 [Table] 特性的类型
        if (!string.IsNullOrWhiteSpace(tableAttribute?.Schema)) return;
        
        // 获取真实表名
        var tableName = tableAttribute?.Name ?? type.Name;

        // 判断是否配置了 Schema，如果是直接采用
        var schema = tableAttribute?.Schema;

        // 获取类型前缀 [TablePrefix] 特性
        var tablePrefixAttribute = type.GetCustomAttribute<TablePreSuffixAttribute>(true);

        // 判断是否启用全局表前后缀支持或个别表自定义了前缀
        if (tablePrefixAttribute != null)
        {
            entityBuilder.ToTable($"{tablePrefixAttribute.Prefix}{tableName}{tablePrefixAttribute.Suffix}", schema);
        }
        else 
        {
            var appDbContextAttribute = DbProvider.GetAppDbContextAttribute(dbContext.GetType());
            // 添加表统一前后缀，排除视图
            if (appDbContextAttribute != null && (!string.IsNullOrWhiteSpace(appDbContextAttribute.TablePrefix) || !string.IsNullOrWhiteSpace(appDbContextAttribute.TableSuffix)))
            {
                var tablePrefix = appDbContextAttribute.TablePrefix?.Trim();
                var tableSuffix = appDbContextAttribute.TableSuffix?.Trim();
                entityBuilder.ToTable($"{tablePrefix}{tableName}{tableSuffix}", schema);
                return;
            }
        }
    }



    /// <summary>
    /// 配置数据库实体类型构建器
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <param name="entityBuilder">实体类型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="dbContextLocator">数据库上下文定位器</param>
    /// <param name="dbContextCorrelationType">数据库实体关联类型</param>
    private static void ConfigureEntityTypeBuilder(this EntityTypeBuilder entityBuilder, IMutableEntityType mutableEntityType, DbContext dbContext)
    {

        if (mutableEntityType.IsOwned())
        {
            return;
        }

        if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }


        // 获取该实体类型的配置类型
        var entityTypeBuilderTypes = dbContextCorrelationType.EntityTypeBuilderTypes
            .Where(u => u.GetInterfaces()
                .Any(i => i.HasImplementedRawGeneric(typeof(IPrivateEntityTypeBuilder<>)) && i.GenericTypeArguments.Contains(entityType)));

        if (!entityTypeBuilderTypes.Any()) return;

        // 调用数据库实体自定义配置
        foreach (var entityTypeBuilderType in entityTypeBuilderTypes)
        {
            var instance = Activator.CreateInstance(entityTypeBuilderType);
            var configureMethod = entityTypeBuilderType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                               .Where(u => u.Name == "Configure"
                                                                    && u.GetParameters().Length > 0
                                                                    && u.GetParameters().First().ParameterType == typeof(EntityTypeBuilder<>).MakeGenericType(entityType))
                                                               .FirstOrDefault();

            configureMethod.Invoke(instance, new object[] { entityBuilder, dbContext, dbContextLocator });
        }
    }


}
