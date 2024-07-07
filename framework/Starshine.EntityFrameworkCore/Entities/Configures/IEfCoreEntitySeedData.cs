namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库种子数据依赖接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEFCoreEntitySeedData<TEntity> 
        where TEntity : class
    {
        /// <summary>
        /// 配置种子数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> HasData();
    }
}