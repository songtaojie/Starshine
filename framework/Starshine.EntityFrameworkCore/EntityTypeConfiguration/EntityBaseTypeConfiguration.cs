using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.Common;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 基础的配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class EntityBaseTypeConfiguration<T, TKey> : EntityTypeConfiguration<T, TKey>
            where T : EntityBase<TKey>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().HasMaxLength(36).HasComment("主键");
        }
    }

    /// <summary>
    /// 基础的配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public abstract class EntityBaseTypeConfiguration<T> : EntityBaseTypeConfiguration<T, long>
            where T : EntityBase
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
        }
    }
}
