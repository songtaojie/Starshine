using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// IConventionPropertyBuilder扩展类
    /// </summary>
    internal static class IConventionPropertyBuilderExtensions
    {
        public static IConventionPropertyBuilder HasPrecision(this IConventionPropertyBuilder propertyBuilder, int precision, int scale)
        {
            //去掉Microsoft.EntityFrameworkCore.Relational包引用
            propertyBuilder.HasColumnType($"decimal({precision},{scale})");
            //propertyBuilder.HasPrecision(precision);
            //propertyBuilder.HasScale(scale);
            return propertyBuilder;
        }

        //public static IConventionPropertyBuilder HasPrecision(this IConventionPropertyBuilder propertyBuilder, int precision)
        //{
        //    if (propertyBuilder.Metadata.CanSetPrecision(precision, ConfigurationSource.DataAnnotation))
        //    {
        //        propertyBuilder.Metadata.SetPrecision(precision, true);
        //        return propertyBuilder;
        //    }
        //    return propertyBuilder;
        //}

        //private static bool CanSetPrecision(this IConventionProperty metadata, int? precision, ConfigurationSource? configurationSource)
        //{
        //    if (!configurationSource.Overrides(metadata.GetPrecisionConfigurationSource()))
        //    {
        //        int? precision2 = metadata.GetPrecision();
        //        int? num = precision;
        //        return precision2.GetValueOrDefault() == num.GetValueOrDefault() & precision2 != null == (num != null);
        //    }
        //    return true;
        //}

        //public static IConventionPropertyBuilder HasScale(this IConventionPropertyBuilder propertyBuilder, int? scale)
        //{
        //    if (propertyBuilder.Metadata.CanSetScale(scale, ConfigurationSource.DataAnnotation))
        //    {
        //        propertyBuilder.Metadata.SetScale(scale, true);
        //        return propertyBuilder;
        //    }
        //    return null;
        //}

        //private static bool CanSetScale(this IConventionProperty metadata, int? scale, ConfigurationSource? configurationSource)
        //{
        //    if (!configurationSource.Overrides(metadata.GetScaleConfigurationSource()))
        //    {
        //        int? scale2 = metadata.GetScale();
        //        int? num = scale;
        //        return scale2.GetValueOrDefault() == num.GetValueOrDefault() & scale2 != null == (num != null);
        //    }
        //    return true;
        //}
    }

}
