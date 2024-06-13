using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hx.DatabaseAccessor.EntityConfiguration
{
    /// <summary>
    /// EntityType配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class, Internal.IPrivateEntity
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
