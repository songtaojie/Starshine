using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// 枚举转换器特性，可以通过注解的形势吧枚举转换成int，string，long
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class EnumConverterAttribute : Attribute
    {

        #region Field
        #endregion

        #region Construct
        /// <summary>
        /// 枚举转换器构造函数
        /// </summary>
        /// <param name="conventionType">要把枚举转换成的类型</param>
        public EnumConverterAttribute(EnumConventionType conventionType)
        {
            ConventionType = conventionType;
        }
        #endregion

        #region Property
        /// <summary>
        /// 类型
        /// </summary>
        public EnumConventionType ConventionType { get; set; } = EnumConventionType.EnumToString;
        /// <summary>
        /// 转换器
        /// </summary>
        public ValueConverter ValueConverter { get; set; }

        #endregion
    }

    /// <summary>
    /// 枚举转成的类型
    /// </summary>
    public enum EnumConventionType
    {
        /// <summary>
        /// 枚举转成字符串
        /// </summary>
        EnumToString,
        /// <summary>
        /// 枚举转成整形
        /// </summary>
        EnumToInt,
        /// <summary>
        /// 枚举转成长整型
        /// </summary>
        EnumToLong
    }
}
