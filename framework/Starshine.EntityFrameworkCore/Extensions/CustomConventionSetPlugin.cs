using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 自定义扩展约定插件
    /// </summary>
    internal class CustomConventionSetPlugin : IConventionSetPlugin
    {
        /// <summary>
        /// 重写约定
        /// </summary>
        /// <param name="conventionSet"></param>
        /// <returns></returns>
        public ConventionSet ModifyConventions(ConventionSet conventionSet)
        {
            conventionSet.PropertyAddedConventions.Add(new EnumConverterAttributeConvention(null));
            conventionSet.PropertyAddedConventions.Add(new DecimalPrecisionAttributeConvention(null));
            conventionSet.PropertyAddedConventions.Add(new ValueConverterAttributeConvention(null));
            return conventionSet;
        }
    }
}
