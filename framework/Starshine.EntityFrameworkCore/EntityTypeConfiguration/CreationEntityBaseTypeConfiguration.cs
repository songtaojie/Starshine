using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.Common;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 带创建时间的配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class CreationEntityBaseTypeConfiguration<T, TKey> : EntityBaseTypeConfiguration<T, TKey>
         where T : CreationEntityBase<TKey>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreateTime).HasComment("创建时间");
            builder.Property(x => x.CreatorId).HasComment("创建者id");
        }
    }

    /// <summary>
    /// 带创建时间的配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class CreationEntityBaseTypeConfiguration<T> : EntityBaseTypeConfiguration<T>
         where T : CreationEntityBase
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreateTime).HasComment("创建时间");
            builder.Property(x => x.CreatorId).HasComment("创建者id");
        }
    }
}
