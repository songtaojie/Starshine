using Starshine.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文关联类型
    /// </summary>
    internal sealed class DbContextCorrelationType
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal DbContextCorrelationType()
        {
            EntityTypes = new List<Type>();
            EntityTypeBuilderTypes = new List<Type>();
            EntitySeedDataTypes = new List<Type>();
            ModelBuilderFilterTypes = new List<Type>();
            EntityMutableTableTypes = new List<Type>();
            ModelBuilderFilterInstances = new List<IPrivateModelBuilderFilter>();
            DbFunctionMethods = new List<MethodInfo>();
        }

        /// <summary>
        /// 关联的数据库上下文
        /// </summary>
        internal Type DbContextLocator { get; set; }

        /// <summary>
        /// 所有关联类型
        /// </summary>
        internal List<Type> Types { get; set; }

        /// <summary>
        /// 实体类型集合
        /// </summary>
        internal List<Type> EntityTypes { get; set; }

        /// <summary>
        /// 实体构建器类型集合
        /// </summary>
        internal List<Type> EntityTypeBuilderTypes { get; set; }

        /// <summary>
        /// 种子数据类型集合
        /// </summary>
        internal List<Type> EntitySeedDataTypes { get; set; }


        /// <summary>
        /// 模型构建筛选器类型集合
        /// </summary>
        internal List<Type> ModelBuilderFilterTypes { get; set; }

        /// <summary>
        /// 可变表实体类型集合
        /// </summary>
        internal List<Type> EntityMutableTableTypes { get; set; }

        /// <summary>
        /// 数据库函数方法集合
        /// </summary>
        internal List<MethodInfo> DbFunctionMethods { get; set; }

    }
}