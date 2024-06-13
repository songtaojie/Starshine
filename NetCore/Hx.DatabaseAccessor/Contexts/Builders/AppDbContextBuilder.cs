﻿using Hx.DatabaseAccessor.Extensions;
using Hx.DatabaseAccessor.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库上下文构建器
    /// </summary>
    internal static class AppDbContextBuilder
    {
        /// <summary>
        /// 数据库实体相关类型
        /// </summary>
        private static readonly List<Type> EntityCorrelationTypes;

        /// <summary>
        /// 数据库函数方法集合
        /// </summary>
        private static readonly List<MethodInfo> DbFunctionMethods;

        /// <summary>
        /// 创建数据库实体方法
        /// </summary>
        private static readonly MethodInfo ModelBuildEntityMethod;

        /// <summary>
        /// 构造函数
        /// </summary>
        static AppDbContextBuilder()
        {

            // 扫描程序集，获取数据库实体相关类型
            EntityCorrelationTypes = Penetrates.EffectiveTypes.Where(t => (typeof(IPrivateEntity).IsAssignableFrom(t) || typeof(IPrivateModelBuilder).IsAssignableFrom(t))
                && t.IsClass && !t.IsAbstract && !t.IsGenericType && !t.IsInterface && !t.IsDefined(typeof(NonAutomaticAttribute), true))
                .ToList();

            if (EntityCorrelationTypes.Count > 0)
            {
                DbContextLocatorCorrelationTypes = new ConcurrentDictionary<Type, DbContextCorrelationType>();

                // 获取模型构建器 Entity<T> 方法
                ModelBuildEntityMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(u => u.Name == nameof(ModelBuilder.Entity) && u.GetParameters().Length == 0);
            }

            // 查找所有数据库函数，必须是公开静态方法，且所在父类也必须是公开静态方法
            DbFunctionMethods = Penetrates.EffectiveTypes
                .Where(t => t.IsAbstract && t.IsSealed && t.IsClass && !t.IsDefined(typeof(NonAutomaticAttribute), true))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m =>m.IsDefined(typeof(QueryableFunctionAttribute), true))).ToList();
        }

        /// <summary>
        /// 配置数据库上下文实体
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        internal static void ConfigureDbContextEntity(ModelBuilder modelBuilder, DbContext dbContext, Type dbContextLocator)
        {
            // 获取当前数据库上下文关联的类型
            var dbContextCorrelationType = GetDbContextCorrelationType(dbContext, dbContextLocator);

            // 如果没有数据，则跳过
            if (!dbContextCorrelationType.EntityTypes.Any()) goto EntityFunctions;

            // 获取当前数据库上下文的 [DbContextAttributes] 特性
            var dbContextType = dbContext.GetType();
            var appDbContextAttribute = DbProvider.GetAppDbContextAttribute(dbContextType);

            // 初始化所有类型
            foreach (var entityType in dbContextCorrelationType.EntityTypes)
            {
                // 创建实体类型
                var entityBuilder = CreateEntityTypeBuilder(entityType, modelBuilder, dbContext, dbContextType, dbContextLocator, dbContextCorrelationType, appDbContextAttribute);
                if (entityBuilder == null) continue;

                // 实体构建成功注入拦截
                LoadModelBuilderOnCreating(modelBuilder, entityBuilder, dbContext, dbContextLocator, dbContextCorrelationType.ModelBuilderFilterInstances);

                // 配置数据库实体类型构建器
                ConfigureEntityTypeBuilder(entityType, entityBuilder, dbContext, dbContextLocator, dbContextCorrelationType);

                // 配置数据库实体种子数据
                ConfigureEntitySeedData(entityType, entityBuilder, dbContext, dbContextLocator, dbContextCorrelationType);

                // 实体完成配置注入拦截
                LoadModelBuilderOnCreated(modelBuilder, entityBuilder, dbContext, dbContextLocator, dbContextCorrelationType.ModelBuilderFilterInstances);
            }

        // 配置数据库函数
        EntityFunctions: ConfigureDbFunctions(modelBuilder, dbContextLocator);
        }

        /// <summary>
        /// 创建实体类型构建器
        /// </summary>
        /// <param name="type">数据库关联类型</param>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="dbContextCorrelationType"></param>
        /// <param name="appDbContextAttribute">数据库上下文特性</param>
        /// <returns>EntityTypeBuilder</returns>
        private static EntityTypeBuilder CreateEntityTypeBuilder(Type type, ModelBuilder modelBuilder, DbContext dbContext, Type dbContextType, Type dbContextLocator, DbContextCorrelationType dbContextCorrelationType, AppDbContextAttribute appDbContextAttribute = null)
        {
            // 反射创建实体类型构建器
            var entityTypeBuilder = ModelBuildEntityMethod.MakeGenericMethod(type).Invoke(modelBuilder, null) as EntityTypeBuilder;

            // 配置动态表名
            if (!ConfigureEntityMutableTableName(type, entityTypeBuilder, dbContext, dbContextLocator, dbContextCorrelationType))
            {
                // 配置实体表名
                ConfigureEntityTableName(type, appDbContextAttribute, entityTypeBuilder, dbContext, dbContextType);
            }

            return entityTypeBuilder;
        }

        /// <summary>
        /// 配置实体表名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="appDbContextAttribute">数据库上下文特性</param>
        /// <param name="entityTypeBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextType">数据库上下文类型</param>
        private static void ConfigureEntityTableName(Type type, AppDbContextAttribute appDbContextAttribute, EntityTypeBuilder entityTypeBuilder, DbContext dbContext, Type dbContextType)
        {
            // 获取表是否定义 [Table] 特性
            var tableAttribute = type.IsDefined(typeof(TableAttribute), true) ? type.GetCustomAttribute<TableAttribute>(true) : default;

            // 排除无键实体或已经贴了 [Table] 特性的类型
            if (!string.IsNullOrWhiteSpace(tableAttribute?.Schema)) return;

            // 获取真实表名
            var tableName = tableAttribute?.Name ?? type.Name;

            // 判断是否是启用了多租户模式，如果是，则获取 Schema
            string dynamicSchema = default;

            // 获取类型前缀 [TablePrefix] 特性
            var tablePrefixAttribute = !type.IsDefined(typeof(TablePrefixAttribute), true)
                ? default
                : type.GetCustomAttribute<TablePrefixAttribute>(true);

            // 判断是否启用全局表前后缀支持或个别表自定义了前缀
            if (tablePrefixAttribute != null || appDbContextAttribute == null)
            {
                entityTypeBuilder.ToTable($"{tablePrefixAttribute?.Prefix}{tableName}", dynamicSchema);
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
                            entityTypeBuilder.ToTable($"{tableName}{tableSuffix}", schema: schema);
                        }
                        else entityTypeBuilder.ToTable($"{tablePrefix}{tableName}{tableSuffix}", dynamicSchema);
                    }
                    else entityTypeBuilder.ToTable($"{tableName}{tableSuffix}", dynamicSchema);

                    return;
                }
            }
        }

        /// <summary>
        /// 配置实体动态表名
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="dbContextCorrelationType">数据库实体关联类型</param>
        private static bool ConfigureEntityMutableTableName(Type entityType, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator, DbContextCorrelationType dbContextCorrelationType)
        {
            var isSet = false;

            // 获取实体动态配置表配置
            var entityMutableTableTypes = dbContextCorrelationType.EntityMutableTableTypes
                .Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IPrivateEntityMutableTable<>)) && i.GenericTypeArguments.Contains(entityType)));

            if (!entityMutableTableTypes.Any()) return isSet;

            // 只应用于扫描的最后配置
            var lastEntityMutableTableType = entityMutableTableTypes.Last();

            var instance = Activator.CreateInstance(lastEntityMutableTableType);
            var getTableNameMethod = lastEntityMutableTableType.GetMethod("GetTableName");
            var tableName = getTableNameMethod.Invoke(instance, new object[] { dbContext, dbContextLocator });
            if (tableName != null)
            {
                // 设置动态表名
                entityBuilder.ToTable(tableName.ToString());
                isSet = true;
            }

            return isSet;
        }

        /// <summary>
        /// 加载模型构建筛选器创建之前拦截
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="modelBuilderFilterInstances">模型构建器筛选器实例</param>
        private static void LoadModelBuilderOnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator, List<IPrivateModelBuilderFilter> modelBuilderFilterInstances)
        {
            if (modelBuilderFilterInstances.Count == 0) return;

            // 创建过滤器
            foreach (var filterInstance in modelBuilderFilterInstances)
            {
                // 执行构建之后
                filterInstance.OnCreating(modelBuilder, entityBuilder, dbContext, dbContextLocator);
            }
        }

        /// <summary>
        /// 加载模型构建筛选器创建之后拦截
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="modelBuilderFilterInstances">模型构建器筛选器实例</param>
        private static void LoadModelBuilderOnCreated(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator, List<IPrivateModelBuilderFilter> modelBuilderFilterInstances)
        {
            if (modelBuilderFilterInstances.Count == 0) return;

            // 创建过滤器
            foreach (var filterInstance in modelBuilderFilterInstances)
            {
                // 执行构建之后
                filterInstance.OnCreated(modelBuilder, entityBuilder, dbContext, dbContextLocator);
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
        private static void ConfigureEntityTypeBuilder(Type entityType, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator, DbContextCorrelationType dbContextCorrelationType)
        {
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

        /// <summary>
        /// 配置数据库实体种子数据
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="dbContextCorrelationType">数据库实体关联类型</param>
        private static void ConfigureEntitySeedData(Type entityType, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator, DbContextCorrelationType dbContextCorrelationType)
        {
            // 获取该实体类型的种子配置
            var entitySeedDataTypes = dbContextCorrelationType.EntitySeedDataTypes
                .Where(u => u.GetInterfaces()
                    .Any(i => i.HasImplementedRawGeneric(typeof(IPrivateEntitySeedData<>)) && i.GenericTypeArguments.Contains(entityType)));

            if (!entitySeedDataTypes.Any()) return;

            var data = new List<object>();

            // 加载种子配置数据
            foreach (var entitySeedDataType in entitySeedDataTypes)
            {
                var instance = Activator.CreateInstance(entitySeedDataType);
                var hasDataMethod = entitySeedDataType.GetMethod("HasData");

                var seedData = ((IList)hasDataMethod.Invoke(instance, new object[] { dbContext, dbContextLocator })).Cast<object>();
                data.AddRange(seedData);
            }

            entityBuilder.HasData(data.ToArray());
        }

        /// <summary>
        /// 配置数据库函数
        /// </summary>
        /// <param name="modelBuilder">模型构建起</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        private static void ConfigureDbFunctions(ModelBuilder modelBuilder, Type dbContextLocator)
        {
            var dbContextFunctionMethods = DbFunctionMethods.Where(u => IsInThisDbContext(dbContextLocator, u));
            if (!dbContextFunctionMethods.Any()) return;

            foreach (var method in dbContextFunctionMethods)
            {
                modelBuilder.HasDbFunction(method);
            }
        }

        /// <summary>
        /// 判断当前类型是否在数据库上下文中
        /// </summary>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="entityCorrelationType">数据库实体关联类型</param>
        /// <returns>bool</returns>
        private static bool IsInThisDbContext(Type dbContextLocator, Type entityCorrelationType)
        {
            // 获取所有祖先类型
            var ancestorTypes = entityCorrelationType.GetAncestorTypes();
            // 获取所有接口
            var interfaces = entityCorrelationType.GetInterfaces().Where(u => typeof(IPrivateEntity).IsAssignableFrom(u) || typeof(IPrivateModelBuilder).IsAssignableFrom(u));

            // 祖先是泛型且泛型参数包含数据库上下文定位器
            if (ancestorTypes.Any(u => u.IsGenericType && u.GenericTypeArguments.Contains(dbContextLocator))) return true;

            // 接口是泛型且泛型参数包含数据库上下文定位器
            if (interfaces.Any(u => u.IsGenericType && u.GenericTypeArguments.Contains(dbContextLocator))) return true;

            return false;
        }

        /// <summary>
        /// 判断当前函数是否在数据库上下文中
        /// </summary>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <param name="method">标识为数据库的函数</param>
        /// <returns>bool</returns>
        private static bool IsInThisDbContext(Type dbContextLocator, MethodInfo method)
        {
            var queryableFunctionAttribute = method.GetCustomAttribute<QueryableFunctionAttribute>(true);

            // 如果数据库上下文定位器为默认定位器且该函数没有定义数据库上下文定位器，则返回 true
            if (dbContextLocator == typeof(MasterDbContextLocator) && queryableFunctionAttribute.DbContextLocators.Length == 0) return true;

            // 判断是否包含当前数据库上下文
            if (queryableFunctionAttribute.DbContextLocators.Contains(dbContextLocator)) return true;

            return false;
        }

        /// <summary>
        /// 数据库上下文定位器关联类型集合
        /// </summary>
        internal static ConcurrentDictionary<Type, DbContextCorrelationType> DbContextLocatorCorrelationTypes;

        /// <summary>
        /// 获取当前数据库上下文关联类型
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <returns>DbContextCorrelationType</returns>
        private static DbContextCorrelationType GetDbContextCorrelationType(DbContext dbContext, Type dbContextLocator)
        {
            // 读取缓存
            return DbContextLocatorCorrelationTypes.GetOrAdd(dbContextLocator, Function(dbContext, dbContextLocator));

            // 本地静态方法
            static DbContextCorrelationType Function(DbContext dbContext, Type dbContextLocator)
            {
                var result = new DbContextCorrelationType { DbContextLocator = dbContextLocator };

                // 获取当前数据库上下文关联类型
                var dbContextEntityCorrelationTypes = EntityCorrelationTypes.Where(u => IsInThisDbContext(dbContextLocator, u));

                // 组装对象
                foreach (var entityCorrelationType in dbContextEntityCorrelationTypes)
                {
                    // 只要继承 IEntityDependency 接口，都是实体
                    if (typeof(IPrivateEntity).IsAssignableFrom(entityCorrelationType))
                    {
                        // 添加实体
                        result.EntityTypes.Add(entityCorrelationType);

                    }

                    if (typeof(IPrivateModelBuilder).IsAssignableFrom(entityCorrelationType))
                    {
                        // 添加模型构建器
                        if (entityCorrelationType.HasImplementedRawGeneric(typeof(IPrivateEntityTypeBuilder<>)))
                        {
                            result.EntityTypeBuilderTypes.Add(entityCorrelationType);
                        }

                        // 添加全局筛选器
                        if (entityCorrelationType.HasImplementedRawGeneric(typeof(IPrivateModelBuilderFilter)))
                        {
                            result.ModelBuilderFilterTypes.Add(entityCorrelationType);

                            // 判断是否是 DbContext 类型，
                            if (typeof(DbContext).IsAssignableFrom(entityCorrelationType))
                            {
                                // 判断是否已经注册了上下文并且是否等于当前上下文
                                if (Penetrates.DbContextDescriptors.Values.Contains(entityCorrelationType) && entityCorrelationType == dbContext.GetType())
                                {
                                    result.ModelBuilderFilterInstances.Add(dbContext as IPrivateModelBuilderFilter);
                                }
                            }
                            else result.ModelBuilderFilterInstances.Add(Activator.CreateInstance(entityCorrelationType) as IPrivateModelBuilderFilter);
                        }

                        // 添加种子数据
                        if (entityCorrelationType.HasImplementedRawGeneric(typeof(IPrivateEntitySeedData<>)))
                        {
                            result.EntitySeedDataTypes.Add(entityCorrelationType);
                        }

                        // 添加动态表类型
                        if (entityCorrelationType.HasImplementedRawGeneric(typeof(IPrivateEntityMutableTable<>)))
                        {
                            result.EntityMutableTableTypes.Add(entityCorrelationType);
                        }
                       
                    }
                }

                return result;
            }
        }
    }
}