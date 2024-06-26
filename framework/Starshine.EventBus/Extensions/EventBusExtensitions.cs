﻿using System;
using System.Reflection;

namespace Starshine.EventBus
{
    /// <summary>
    /// 事件总线拓展类
    /// </summary>
    public static class EventBusExtensitions
    {
        /// <summary>
        /// 将事件枚举 Id 转换成字符串对象
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string ParseToString(this Enum em)
        {
            var enumType = em.GetType();
            return $"{enumType.Assembly.GetName().Name};{enumType.FullName}.{em}";
        }

        /// <summary>
        /// 将事件枚举字符串转换成枚举对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Enum? ParseToEnum(this string str)
        {
            var assemblyName = str[..str.IndexOf(';')];
            var fullName = str[(str.IndexOf(';') + 1)..str.LastIndexOf('.')];
            var name = str[(str.LastIndexOf('.') + 1)..];
            var type = Assembly.Load(assemblyName).GetType(fullName);
            if (type == null) return default;
            return Enum.Parse(type, name) as Enum;
        }
    }
}

