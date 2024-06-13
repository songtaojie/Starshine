using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Reflection;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 枚举转换器的属性约定
    /// </summary>
    internal class EnumConverterAttributeConvention : PropertyAttributeConventionBase<EnumConverterAttribute>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencies"></param>
        public EnumConverterAttributeConvention(ProviderConventionSetBuilderDependencies dependencies) : base(dependencies)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyBuilder"></param>
        /// <param name="attribute"></param>
        /// <param name="clrMember"></param>
        /// <param name="context"></param>
        protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, EnumConverterAttribute attribute, MemberInfo clrMember, IConventionContext context)
        {
            ConverterMappingHints mappingHints = null;
            switch (attribute.ConventionType)
            {
                case EnumConventionType.EnumToString:
                    var toStringGeneric = typeof(EnumToStringConverter<>);
                    var toStringPropInfo = clrMember.DeclaringType.GetProperty(clrMember.Name);
                    var toStringType = toStringGeneric.MakeGenericType(toStringPropInfo.PropertyType);
                    object toStringInstance = Activator.CreateInstance(toStringType, mappingHints);
                    propertyBuilder.HasConversion(toStringInstance as ValueConverter, true);
                    //propertyBuilder.Metadata.SetValueConverter(toStringInstance as ValueConverter, true);
                    break;
                case EnumConventionType.EnumToInt:
                    var toIntGeneric = typeof(EnumToNumberConverter<,>);
                    var toIntPropInfo = clrMember.DeclaringType.GetProperty(clrMember.Name);
                    var toIntType = toIntGeneric.MakeGenericType(toIntPropInfo.PropertyType, typeof(int));
                    object toIntInstance = Activator.CreateInstance(toIntType, mappingHints);
                    propertyBuilder.HasConversion(toIntInstance as ValueConverter, true);
                    break;
                case EnumConventionType.EnumToLong:
                    var toLongGeneric = typeof(EnumToNumberConverter<,>);
                    var toLongPropInfo = clrMember.DeclaringType.GetProperty(clrMember.Name);
                    var toLongType = toLongGeneric.MakeGenericType(toLongPropInfo.PropertyType, typeof(long));
                    object toLongInstance = Activator.CreateInstance(toLongType, mappingHints);
                    propertyBuilder.HasConversion(toLongInstance as ValueConverter, true);
                    break;
            }

        }
    }
}
