using Starshine.DatabaseAccessor;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    ///// <summary>
    ///// 自定义约束扩展类
    ///// </summary>
    //public static class CustomConventionServiceCollectionExtensions
    //{
    //    /// <summary>
    //    /// 添加自定义约定
    //    /// </summary>
    //    /// <param name="optionsBuilder"></param>
    //    /// <returns></returns>
    //    public static DbContextOptionsBuilder AddCustomConvention(this DbContextOptionsBuilder optionsBuilder)
    //    {
    //        var extension = GetOrCreateExtension(optionsBuilder);
    //        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
    //        return optionsBuilder;
    //    }

    //    private static IDbContextOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        IDbContextOptionsExtension optionsExtension = optionsBuilder.Options.FindExtension<CustomDbContextOptionsExtension>();
    //        if (optionsExtension == null)
    //        {
    //            return new CustomDbContextOptionsExtension();
    //        }
    //        return optionsExtension;
    //    }

    //}
}
