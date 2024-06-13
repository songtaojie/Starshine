using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// 身份证
        /// </summary>
        public const string Regex_IDCard = @"\d{17}[\d|X]|\d{15}";
        /// <summary>
        /// 邮编
        /// </summary>
        public const string Regex_ZipCode = @"\d{6}";
        #region 字符串
        /// <summary>
        /// 比较字符串，忽略大小写
        /// </summary>
        public static bool AreEqual(string value1, string value2)
        {
            return string.Equals(value1, value2, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// 判断当前字符串是否为Y
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsYes(string value)
        {
            return Helper.AreEqual(value, "Y");
        }
        /// <summary>
        /// 判断当前字符串是否为N
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNo(string value)
        {
            return Helper.AreEqual(value, "N");
        }
        /// <summary>
        /// 批量比较字符串
        /// </summary>
        /// <param name="andOr">True:比较时以And作为标准.False:比较时以Or作为标准.</param>
        /// <param name="firstValue">比较值</param>
        /// <param name="valueArray">比较字符串集合</param>
        /// <returns></returns>
        public static bool AreEqual(bool andOr, string firstValue, params string[] valueArray)
        {
            if (valueArray.Length == 0) return false;
            foreach (string valueItem in valueArray)
            {
                bool returnValue = string.Equals(firstValue, valueItem, StringComparison.CurrentCultureIgnoreCase);
                if (andOr && !returnValue) return false;
                if (!andOr && returnValue) return true;
            }
            return andOr;
        }
        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStr(object value)
        {
            if (value != null && value != DBNull.Value)
            {
                return string.Format("{0}", value);
            }
            return string.Empty;
        }
        /// <summary>
        /// 将纯文本中的回车换行转换为Web中的<br/>标签
        /// </summary>
        /// <param name="source">TextArea带回车换行的文本</param>
        /// <returns></returns>
        public static string NlTobr(string source)
        {
            if (string.IsNullOrEmpty(source)) return "";
            return source.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        }
        /// <summary>
        /// 将对象转换成16进制的字符形式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(object value)
        {
            StringBuilder builder = new StringBuilder();
            byte[] byteList = Encoding.Unicode.GetBytes(string.Format("{0}", value));
            foreach (byte b in byteList)
            {
                builder.Append(string.Format("{0:X2}", b));
            }
            return builder.ToString();
        }
        /// <summary>
        /// 将16进制的字符形式转换成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromHex(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToUpper();
                List<byte> arrayList = new List<byte>();
                for (int index = 0; index < value.Length - 1; index += 2)
                {
                    arrayList.Add(Convert.ToByte(ToInt(value[index]) * 16 + ToInt(value[index + 1])));
                }
                return Encoding.Unicode.GetString(arrayList.ToArray());
            }
            return value;
        }
        private static int ToInt(char ch)
        {
            if (ch >= 'A') return ch - 'A' + 10;
            return ch - '0';
        }

        /// <summary>
        /// 获取雪花ID
        /// </summary>
        /// <returns></returns>
        public static long GetLongSnowId()
        {
            return Snowflake.Instance().GetId();
        }

        /// <summary>
        /// 获取雪花ID
        /// </summary>
        /// <returns></returns>
        public static string GetSnowId()
        {
            return GetLongSnowId().ToString();
        }
        #endregion

        #region 日期
        /// <summary>
        /// 
        /// </summary>
        /// <param name="absID">日期格式为2018-01或者201801或者2018/01</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private static void GetYearAndMonth(string absID, out int year, out int month)
        {
            if (string.IsNullOrEmpty(absID)) throw new Exception("无效的期间字符串.");
            string yearStr = null, monthStr = null;
            if (absID.Length == 6)
            {
                yearStr = absID.Substring(0, 4);
                monthStr = absID.Substring(4);
            }
            else if (absID.Length == 7 && absID.Contains("-"))
            {
                yearStr = absID.Split('-')[0];
                monthStr = absID.Split('-')[1];
            }
            else if (absID.Length == 7 && absID.Contains("/"))
            {
                yearStr = absID.Split('/')[0];
                monthStr = absID.Split('/')[1];
            }
            if (string.IsNullOrEmpty(yearStr) || string.IsNullOrEmpty(monthStr) || yearStr.Length != 4) throw new Exception("无效的期间字符串.");
            year = int.Parse(yearStr);
            month = int.Parse(monthStr);
        }
        /// <summary>
        /// 返回当前日期第一天的日期对象
        /// </summary>
        /// <param name="absID"></param>
        /// <returns></returns>
        public static DateTime GetFirstDate(string absID)
        {
            int year; int month;
            GetYearAndMonth(absID, out year, out month);
            return new DateTime(year, month, 1);
        }
        /// <summary>
        /// 返回当前日期对应的最后一天的日期对象
        /// </summary>
        /// <param name="absID"></param>
        /// <returns></returns>
        public static DateTime GetLastDate(string absID)
        {
            int year; int month;
            GetYearAndMonth(absID, out year, out month);
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }
        /// <summary>
        /// 获取指定的年月中那一月的总的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetDays(int year, int month)
        {
            int num = IsLeapYear(year) ? 29 : 28;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: return 31;
                case 4:
                case 6:
                case 9:
                case 11: return 30;
                case 2: return num;
                default: break;
            }
            throw new Exception(string.Format("无法解析日期(年[{0}]月[{1}]).", year, month));
        }
        /// <summary>
        /// 判断所给的年是否是闰年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0);
        }
        /// <summary>
        /// 日期是一年中所在的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }
        /// <summary>
        /// 相对开始日期，当前是第几周
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetWeekBetween(DateTime begin, DateTime end)
        {
            DateTime ST = begin.AddDays(-(int)begin.DayOfWeek);
            return Convert.ToInt32((end - ST).Days) / 7;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1992, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        #endregion

        #region 数值
        /// <summary>
        /// 比较两个十进制的数值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static int Compare(decimal value1, decimal value2)
        {
            decimal rate = value1 - value2;
            if (Math.Abs(rate) < Precision)
            {
                return 0;
            }
            return rate < 0 ? -1 : 1;
        }
        private static decimal Precision
        {
            get { return 1e-10m; }
        }
        #endregion
        /// <summary>
        /// 判断是否是Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(string str)
        {
            try
            {
                Guid guid = new Guid(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 克隆集合对象
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TInfo> Clone<TInfo>(List<TInfo> list)
        {
            List<TInfo> newList = new List<TInfo>();
            foreach (TInfo info in list)
            {
                newList.Add(Clone(info));
            }

            return newList;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="TInfo"></typeparam>
        /// <param name="info">info必须具有[Serializable]标签</param>
        /// <returns></returns>
        public static TInfo Clone<TInfo>(TInfo info)
        {
            using (MemoryStream ms = new MemoryStream(1000))
            {
                object CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, info);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深度副本)
                CloneObject = bf.Deserialize(ms);
                // 关闭流
                ms.Close();
                return (TInfo)CloneObject;
            }
        }

        /// <summary>
        /// 根据后缀获取MIME
        /// </summary>
        /// <param name="fileExtension">带.后缀</param>
        /// <returns></returns>
        public static string GuessMIME(string fileExtension)
        {
            string ext = string.IsNullOrEmpty(fileExtension) ? "" : fileExtension.ToLower();
            switch (ext)
            {
                case ".htm":
                case ".html":
                    return "text/HTML";
                case ".log":
                case ".txt":
                    return "text/plain";
                case ".avi":
                    return "video/avi";
                case ".mp4":
                    return "video/mp4";
                case ".mpg":
                case ".mpeg":
                    return "video/mpeg";
                case ".ogv":
                    return "video/ogg";
                case ".mov":
                    return "video/quicktime";
                case ".webm":
                    return "video/webm";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".png":
                    return "image/png";
                case ".webp":
                    return "image/webp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".ogg":
                    return "audio/ogg";
                case ".rtf":
                    return "application/rtf";
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/ms-word";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".xml":
                    return "application/xml";
                case ".zip":
                    return "application/zip";
                case ".rar":
                    return "application/x-rar-compressed";
                case ".svg":
                    return "image/svg+xml";
                case ".ttf":
                    return "application/x-font-ttf";
                case ".woff":
                case ".woff2":
                    return "application/font-woff";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// DataRow 转 Dictionary
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Dictionary<string, object> DataRowToDictionary(DataRow row)
        {
            if (row == null) return null;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn c in row.Table.Columns)
            {
                dic.Add(c.ColumnName, row[c.ColumnName]);
            }
            return dic;
        }

        #region 温度
        /// <summary>
        /// 摄氏温度转换成华氏温度
        /// </summary>
        /// <param name="temperatureCelsius"></param>
        /// <returns></returns>
        public static double CelsiusToFahrenheit(string temperatureCelsius)
        {
            double celsius = System.Double.Parse(temperatureCelsius);
            return (celsius * 9 / 5) + 32;
        }
        /// <summary>
        /// 华氏温度转换成摄氏温度
        /// </summary>
        /// <param name="temperatureFahrenheit"></param>
        /// <returns></returns>
        public static double FahrenheitToCelsius(string temperatureFahrenheit)
        {
            double fahrenheit = System.Double.Parse(temperatureFahrenheit);
            return (fahrenheit - 32) * 5 / 9;
        }
        #endregion
    }
}
