using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 类型帮助类
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        /// 最小的时间对象
        /// </summary>
        public readonly static DateTime MIN_DATETIME = new DateTime(1900, 1, 1);
        /// <summary>
        /// 设置最大的时间对象
        /// </summary>
        public readonly static DateTime MAX_DATETIME = new DateTime(2079, 1, 1);
        /// <summary>
        /// 最小的十进制数值
        /// </summary>
        public readonly static decimal MIN_DECIMAL = -9999999999.999999999m;
        /// <summary>
        /// 最大的十进制数值
        /// </summary>
        public readonly static decimal MAX_DECIMAL = 9999999999.999999999m;
        /// <summary>
        /// 判断给定的值是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true代表为空，false代表不为空</returns>
        public static bool IsNull(object value)
        {
            return (value == null || value == DBNull.Value);
        }
        /// <summary>
        /// 判断给定的值是否不为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true代表不为空;false代表为空</returns>
        public static bool IsNotNull(object value)
        {
            return !IsNull(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 64位带符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetInt64(object value)
        {
            return IsNull(value) ? default(long) : Convert.ToInt64(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 64位带符号整数，可返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? GetNInt64(object value)
        {
            if (IsNull(value)) return null;
            return (long)value;
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 32位带符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetInt32(object value)
        {
            return IsNull(value) ? default(int) : Convert.ToInt32(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 32位带符号整数，可返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? GetNInt32(object value)
        {
            if (IsNull(value)) return null;
            return (int)value;
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 16位带符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 GetInt16(object value)
        {
            return IsNull(value) ? default(Int16) : Convert.ToInt16(value);
        }
        /// <summary>
        /// 将指定值表示形式转换为等效的 16位带符号整数，可返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short? GetNInt16(object value)
        {
            if (IsNull(value)) return null;
            return (Int16)value;
        }
        /// <summary>
        /// 将指定的对象栓换成十进制数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object value)
        {
            return IsNull(value) ? default(decimal) : Convert.ToDecimal(value);
        }
        /// <summary>
        /// 将指定的对象栓换成十进制数值，可返回null值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? GetNDecimal(object value)
        {
            if (IsNull(value)) return null;
            return Convert.ToDecimal(value);
        }
        /// <summary>
        /// 是否超出了十进制的范围
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOverDecimal(decimal value)
        {
            return Helper.Compare(value, MAX_DECIMAL) > 0 || Helper.Compare(value, MIN_DECIMAL) < 0;
        }
        /// <summary>
        /// 将指定的对象转换成时间,如果给定的值为null则返回当前时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object value)
        {
            return IsNull(value) ? DateTime.Today : Convert.ToDateTime(value);
        }
        /// <summary>
        /// 将指定的对象转换成时间,如果给定的值为null则返回null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetNDateTime(object value)
        {
            if (IsNull(value)) return null;
            return Convert.ToDateTime(value);
        }
        /// <summary>
        /// 将制定的对象转换成等效的字符串形式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetString(object value)
        {
            return IsNull(value) ? default(string) : Convert.ToString(value);
        }
        /// <summary>
        /// 判断该值是否等于Y
        /// </summary>
        /// <param name="value">要与Y比较的值</param>
        /// <returns></returns>
        public static bool GetBoolean(object value)
        {
            return TypeHelper.GetBoolean(value, "Y");
        }
        /// <summary>
        /// 判断对象是否与另一个字符串相等
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool GetBoolean(object value, string compare)
        {
            return Helper.AreEqual(TypeHelper.GetString(value), compare);
        }

        /// <summary>
        /// 取绝对值
        /// </summary>
        public static decimal Abs(decimal dec)
        {
            return (Helper.Compare(dec, 0m) == -1 ? -dec : dec);
        }
        /// <summary>
        /// 取绝对值, 且等于0时,使用1替换
        /// </summary>
        public static decimal Abs(decimal dec, decimal replace)
        {
            int compare = Helper.Compare(dec, 0m);
            return (compare == -1 ? -dec : (compare == 0 ? replace : dec));
        }
    }
}
