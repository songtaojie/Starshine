// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Starshine.JsonSerialization;
/// <summary>
/// 时间格式化
/// </summary>
public class SystemTextJsonDateTimeJsonConverter : JsonConverter<DateTime>
{
  /// <summary>
  /// 时间格式化
  /// 使用内置的时间格式化
  /// </summary>
  public SystemTextJsonDateTimeJsonConverter() : this(default)
  { }

  /// <summary>
  /// 时间格式化
  /// </summary>
  /// <param name="format">格式化字符串</param>
  public SystemTextJsonDateTimeJsonConverter(string? format = "yyyy-MM-dd HH:mm:ss")
  {
    Format = format;
  }
  /// <summary>
  /// 获取或设置DateTime格式
  /// <para>默认为: yyyy-MM-dd HH:mm:ss</para>
  /// </summary>           
  public string? Format { get; private set; }

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
public class SystemTextJsonNullableDateTimeJsonConverter : JsonConverter<DateTime?>
{
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public SystemTextJsonNullableDateTimeJsonConverter():this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public SystemTextJsonNullableDateTimeJsonConverter(string? format = "yyyy-MM-dd HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// 时间格式化格式
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null) writer.WriteNullValue();
        else writer.WriteStringValue(value.Value.ToString(Format));
    }
}
