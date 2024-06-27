// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
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
    static StarshineEntityTypeBuilderExtensions()
    {
        _entityMutableTableTypes = App.EffectiveTypes.Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IEntityMutableTable<>))));

    }

    public static void ConfigureByConvention(this EntityTypeBuilder b)
    {
        b.TryConfigureConcurrencyStamp();
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

    public static void TryConfigureTableName(this EntityTypeBuilder b, DbContext dbContext)
    {
        if (!b.TryConfigureMutableTableName(dbContext))
        {
            b.Property(nameof(IMultiTenant.TenantId))
                .IsRequired(false)
                .HasColumnName(nameof(IMultiTenant.TenantId));
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
    /// <param name="appDbContextAttribute">数据库上下文特性</param>
    /// <param name="entityBuilder">实体类型构建器</param>
    /// <param name="dbContext">数据库上下文</param>
    private static void TryConfigureTableName(this EntityTypeBuilder entityBuilder, StarshineDbContextAttribute appDbContextAttribute, DbContext dbContext)
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

        // 判断是否是启用了多租户模式，如果是，则获取 Schema
        string? dynamicSchema = default;

        // 获取类型前缀 [TablePrefix] 特性
        var tablePrefixAttribute = !type.IsDefined(typeof(TablePrefixAttribute), true)
            ? default
            : type.GetCustomAttribute<TablePrefixAttribute>(true);

        // 判断是否启用全局表前后缀支持或个别表自定义了前缀
        if (tablePrefixAttribute != null || appDbContextAttribute == null)
        {
            entityBuilder.ToTable($"{tablePrefixAttribute?.Prefix}{tableName}", dynamicSchema);
        }
        else
        {
            // 添加表统一前后缀，排除视图
            if (!string.IsNullOrWhiteSpace(appDbContextAttribute.TablePrefix) || !string.IsNullOrWhiteSpace(appDbContextAttribute.TableSuffix))
            {
                var tablePrefix = appDbContextAttribute.TablePrefix;
                var tableSuffix = appDbContextAttribute.TableSuffix;

                if (!string.IsNullOrWhiteSpace(tablePrefix))
                {
                    // 如果前缀中找到 . 字符
                    if (tablePrefix.IndexOf(".") > 0)
                    {
                        var schema = tablePrefix.EndsWith(".") ? tablePrefix[0..^1] : tablePrefix;
                        entityBuilder.ToTable($"{tableName}{tableSuffix}", schema: schema);
                    }
                    else entityBuilder.ToTable($"{tablePrefix}{tableName}{tableSuffix}", dynamicSchema);
                }
                else entityBuilder.ToTable($"{tableName}{tableSuffix}", dynamicSchema);

                return;
            }
        }
    }

}
