// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.Extensions;
using Starshine.EntityFrameworkCore.Extensions.LinqBuilder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Starshine.EntityFrameworkCore.Modeling;
/// <summary>
/// EntityTypeBuilder扩展
/// </summary>
public static class StarshineEntityTypeBuilderExtensions
{

    private static IEnumerable<Type> _entityMutableTableTypes;
    

    static StarshineEntityTypeBuilderExtensions()
    {
        _entityMutableTableTypes = StarshineApp.EffectiveTypes.Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IEntityMutableTable<>))));

    }

    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="b"></param>
    /// <param name="dbContext">数据库上下文</param>
    public static void ConfigureByConvention(this EntityTypeBuilder b, DbContext dbContext)
    {
        b.TryConfigureTableName(dbContext);
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
        lastEntityMutableTableType = lastEntityMutableTableType.MakeGenericType(entityBuilder.Metadata.ClrType);
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

        // 排除已经贴了 [Table] 特性的类型
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
            var appDbContextAttribute = DatabaseProviderHelper.GetStarshineDbContextAttribute(dbContext.GetType());
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
}
