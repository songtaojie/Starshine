using Microsoft.EntityFrameworkCore;
using Starshine.EntityFrameworkCore.Internal;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 动态表名依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityMutableTable<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        StarshineTableName GetTableName(DbContext dbContext);
    }

    /// <summary>
    /// 表名
    /// </summary>
    public class StarshineTableName
    { 
        /// <summary>
        /// 表名称
        /// </summary>
        public string? TableName {  get; set; } = null;

        /// <summary>
        /// Default value: null.
        /// </summary>
        public string? Schema { get; set; } = null;
    }
}