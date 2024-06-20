using Starshine.EntityFrameworkCore.Internal;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    public interface IModelBuilderFilter : IModelBuilderFilter<DefaultDbContextProvider>
    {
    }

    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    public interface IModelBuilderFilter<TDbContextLocator1> : IPrivateModelBuilderFilter
        where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    /// <summary>
    /// 数据库模型构建筛选器依赖接口
    /// </summary>
    /// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
    /// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
    public interface IModelBuilderFilter<TDbContextLocator1, TDbContextLocator2> : IPrivateModelBuilderFilter
        where TDbContextLocator1 : class, IDbContextLocator
        where TDbContextLocator2 : class, IDbContextLocator
    {
    }
}