namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// </summary>
    public abstract class StatusEntityBase : StatusEntityBase<string>
    {
    }

    /// <summary>
    /// 数据库实体依赖基类（使用默认的数据库上下文定位器）
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    public abstract class StatusEntityBase<TKeyType> : StatusEntityBase<TKeyType, MasterDbContextLocator>, IEntity
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1> : Internal.PrivateStatusEntityBase<TKeyType>,IEntity<TDbContextLocator1>
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库实体依赖基类
    /// </summary>
    /// <typeparam name="TKeyType">主键类型</typeparam>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public abstract class StatusEntityBase<TKeyType, TDbContextLocator1, TDbContextLocator2> : Internal.PrivateStatusEntityBase<TKeyType>,IEntity<TDbContextLocator1, TDbContextLocator2>
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum StatusEntityEnum
    {
        /// <summary>
        /// 是
        /// </summary>
        Yes,
        /// <summary>
        /// 否
        /// </summary>
        No
    }

}