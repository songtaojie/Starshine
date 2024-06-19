
namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库实体依赖基接口（使用默认的数据库上下文定位器）
    /// </summary>
    public interface IEntity : IEntity<MasterDbContextLocator>
    {
    }

    /// <summary>
    /// 数据库实体依赖基接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IEntity<TDbContextLocator1> : Internal.IPrivateEntity
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体依赖基接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IEntity<TDbContextLocator1, TDbContextLocator2> : Internal.IPrivateEntity
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }

}