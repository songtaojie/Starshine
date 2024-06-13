using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hx.Common
{
    /// <summary>
    /// 时间格式化
    /// </summary>
    public class SystemTextJsonDateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// 时间格式化
        /// 使用内置的时间格式化
        /// </summary>
        public SystemTextJsonDateTimeJsonConverter():this(default)
        {}

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="format">格式化字符串</param>
        public SystemTextJsonDateTimeJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
        {
            Format = format;
        }
        /// <summary>
        /// 获取或设置DateTime格式
        /// <para>默认为: yyyy-MM-dd HH:mm:ss</para>
        /// </summary>           
        public string Format { get; private set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    return date;
            }
            return reader.GetDateTime();
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
    /// <summary>
    /// 可空的时间格式化
    /// </summary>
    public class SystemTextJsonDateTimeNullJsonConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// 时间格式化
        /// 使用内置的时间格式化
        /// </summary>
        public SystemTextJsonDateTimeNullJsonConverter() : this(default)
        { }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="format">格式化字符串</param>
        public SystemTextJsonDateTimeNullJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
        {
            Format = format;
        }

        /// <summary>
        /// 时间格式化格式
        /// </summary>
        public string Format { get; private set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return string.IsNullOrEmpty(reader.GetString()) ? default(DateTime?) : DateTime.Parse(reader.GetString());
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null) writer.WriteNullValue();
            else writer.WriteStringValue(value.Value.ToString(Format));
        }
    }
}
