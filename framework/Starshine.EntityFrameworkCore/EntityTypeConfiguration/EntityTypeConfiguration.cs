using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Starshine.Common;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// EntityType配置
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public class EntityTypeConfiguration<T,TKey> : IEntityTypeConfiguration<T>
        where T : class, IEntity<TKey>
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
        }
    }
}
