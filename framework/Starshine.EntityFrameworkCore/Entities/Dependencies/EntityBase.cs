namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// 默认主键类型为string
    /// </summary>
    public abstract class EntityBase : EntityBase<string>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    public abstract class EntityBase<TKeyType> : EntityBase<TKeyType, DefaultDbContextTypeProvider>, IEntity
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public abstract class EntityBase<TKeyType, TDbContextLocator1> : Internal.PrivateEntityBase<TKeyType>, IEntity<TDbContextLocator1>
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public abstract class EntityBase<TKeyType, TDbContextLocator1, TDbContextLocator2> : Internal.PrivateEntityBase<TKeyType>, IEntity<TDbContextLocator1, TDbContextLocator2>
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }
}