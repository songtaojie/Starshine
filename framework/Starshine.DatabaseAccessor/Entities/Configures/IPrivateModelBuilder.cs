using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Starshine.DatabaseAccessor.Internal
{
    /// <summary>
    /// 数据库模型构建器依赖（禁止直接继承）
    /// </summary>
    /// <remarks>
    /// 对应 <see cref="Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)"/>
    /// </remarks>
    public interface IPrivateModelBuilder
    {
    }

    /// <summary>
    /// 数据库种子数据依赖接口（禁止外部继承）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPrivateEntitySeedData<TEntity> : IPrivateModelBuilder
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 配置种子数据
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        /// <returns></returns>
        IEnumerable<TEntity> HasData(DbContext dbContext, Type dbContextLocator);
    }

    /// <summary>
    /// 动态表名依赖接口（禁止外部继承）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPrivateEntityMutableTable<TEntity> : IPrivateModelBuilder
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        string GetTableName(DbContext dbContext, Type dbContextLocator);
    }

    /// <summary>
    /// 数据库实体类型配置依赖接口（禁止外部继承）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPrivateEntityTypeBuilder<TEntity> : IPrivateModelBuilder
        where TEntity : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 实体类型配置
        /// </summary>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        void Configure(EntityTypeBuilder<TEntity> entityBuilder, DbContext dbContext, Type dbContextLocator);
    }


    /// <summary>
    /// 数据库模型构建筛选器依赖接口（禁止外部继承）
    /// </summary>
    public interface IPrivateModelBuilderFilter : IPrivateModelBuilder
    {
        /// <summary>
        /// 模型构建之前
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator);

        /// <summary>
        /// 模型构建之后
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="entityBuilder">实体构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="dbContextLocator">数据库上下文定位器</param>
        void OnCreated(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator) { }
    }
}