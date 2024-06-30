namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文定位器
    /// </summary>
    public interface IDbContextTypeProvider
    {
        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        Type GetDbContextType(Type dbContextType);
    }
}