// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Starshine.JsonSerialization;
/// <summary>
/// 解决 long 精度问题
/// </summary>
public class SystemTextJsonLongToStringJsonConverter : JsonConverter<long>
{
    private long MAX_SAFE_INTEGER = 9007199254740991;
    /// <summary>
    /// 构造函数
    /// </summary>
    public SystemTextJsonLongToStringJsonConverter()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overMaxSafeNumber"></param>
    public SystemTextJsonLongToStringJsonConverter(bool overMaxSafeNumber = false)
    {
        OverMaxSafeNumber = overMaxSafeNumber;
    }

    /// <summary>
    /// 是否超过最大安全长度时 再处理
    /// </summary>
    public bool OverMaxSafeNumber { get; set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
                ? Convert.ToInt64(reader.GetString())
                : reader.GetInt64();
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        if (OverMaxSafeNumber && value < MAX_SAFE_INTEGER)
        {
            writer.WriteNumberValue(value);
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

/// <summary>
/// 解决 long? 精度问题
/// </summary>
public class SystemTextJsonNullableLongToStringJsonConverter : JsonConverter<long?>
{
    private long MAX_SAFE_INTEGER = 9007199254740991;
    /// <summary>
    /// 构造函数
    /// </summary>
    public SystemTextJsonNullableLongToStringJsonConverter()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overMaxSafeNumber"></param>
    public SystemTextJsonNullableLongToStringJsonConverter(bool overMaxSafeNumber = false)
    {
        OverMaxSafeNumber = overMaxSafeNumber;
    }

    /// <summary>
    /// 是否超过最大安全长度时 再处理
    /// </summary>
    public bool OverMaxSafeNumber { get; set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        { 
            var strResult = reader.GetString();
            if (string.IsNullOrWhiteSpace(strResult)) return null;
            return Convert.ToInt64(strResult);
        }
        return reader.GetInt64();
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
    {
        if (value == null) writer.WriteNullValue();
        else
        {
            var newValue = value.Value;
            if (OverMaxSafeNumber && newValue < MAX_SAFE_INTEGER)
            {
                writer.WriteNumberValue(newValue);
            }
            else
            {
                writer.WriteStringValue(newValue.ToString());
            }
        }
    }
}
