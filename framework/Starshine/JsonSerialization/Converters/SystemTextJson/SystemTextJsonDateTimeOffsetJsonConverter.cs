// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Starshine.JsonSerialization;

/// <summary>
/// DateTimeOffset 类型序列化
/// </summary>
public class SystemTextJsonDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SystemTextJsonDateTimeOffsetJsonConverter() : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public SystemTextJsonDateTimeOffsetJsonConverter(string? format = "yyyy-MM-dd HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    /// <param name="outputToLocalDateTime"></param>
    public SystemTextJsonDateTimeOffsetJsonConverter(string format, bool outputToLocalDateTime)
    {
        Format = format;
        Localized = outputToLocalDateTime;
    }

    /// <summary>
    /// 时间格式化格式
    /// </summary>
    public string? Format { get; private set; }

    /// <summary>
    /// 是否输出为为当地时间
    /// </summary>
    public bool Localized { get; private set; } = false;

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        DateTime date;
        if (reader.TokenType == JsonTokenType.String && DateTime.TryParse(reader.GetString(), out date))
        {
        }
        return reader.GetDateTime();
        return DateTime.SpecifyKind(Penetrates.ConvertToDateTime(ref reader), Localized ? DateTimeKind.Local : DateTimeKind.Utc);
    }
    private static DateTime ConvertToDateTime(ref Utf8JsonReader reader)
    {
        // 处理时间戳自动转换
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var longValue))
        {
            return longValue.ConvertToDateTime();
        };

        var stringValue = reader.GetString();

        // 处理时间戳自动转换
        if (long.TryParse(stringValue, out var longValue2))
        {
            return longValue2.ConvertToDateTime();
        }

        return Convert.ToDateTime(stringValue);
    }
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        // 判断是否序列化成当地时间
        var formatDateTime = Localized ? value.ConvertToDateTime() : value;
        writer.WriteStringValue(formatDateTime.ToString(Format));
    }
}

/// <summary>
/// DateTimeOffset? 类型序列化
/// </summary>
[SuppressSniffer]
public class SystemTextJsonNullableDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset?>
