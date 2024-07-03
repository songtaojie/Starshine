using System.Text;
using System.Threading;
using Starshine.Common;
namespace Starshine.Extensions
{
    /// <summary>
    /// string类型扩展
    /// </summary>
    [SkipScan]
    public static class StringExtensions
    {
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitlePascal(this string str)
        {
            if (str == null) return string.Empty;

            int iLen = str.Length;
            return iLen == 0
                ? string.Empty
                : iLen == 1
                    ? str.ToLower()
                    : str[0].ToString().ToLower() + str.Substring(1);
        }

        /// <summary>
        /// 字符串转为bool值
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <returns>如果转换成功，返回转换后的值，否则返回null</returns>
        public static bool? ToBool(this string value)
        {
            bool.TryParse(value, out bool r);
            bool? result = r;
            return result;
        }

        /// <summary>
        /// 将字符串URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
        }
    }
}
