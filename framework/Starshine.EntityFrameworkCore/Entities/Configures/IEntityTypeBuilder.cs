using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;
using Starshine.Common;
namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库实体类型配置依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityTypeBuilder<TEntity> 
        where TEntity : class, new()
    {
        /// <summary>
        /// 实体类型配置
        /// </summary>
        /// <param name="entityBuilder">实体类型构建器</param>
        /// <param name="dbContext">数据库上下文</param>
        void Configure(EntityTypeBuilder<TEntity> entityBuilder, DbContext dbContext);
    }
}