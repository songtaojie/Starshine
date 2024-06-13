// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Starshine.JsonSerialization;
/// <summary>
/// 时间转换工具类
/// </summary>
internal static class DateTimeConvertUtils
{
    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    internal static DateTime ConvertToDateTime(ref Utf8JsonReader reader)
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
    /// 转换
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    internal static DateTime ConvertToDateTime(ref JsonReader reader)
    {
        if (reader.TokenType == JsonToken.Integer)
        {
            return JValue.ReadFrom(reader).Value<long>().ConvertToDateTime();
        }

        var stringValue = JValue.ReadFrom(reader).Value<string>();

        // 处理时间戳自动转换
        if (long.TryParse(stringValue, out var longValue2))
        {
            return longValue2.ConvertToDateTime();
        }

        return Convert.ToDateTime(stringValue);
    }
}
