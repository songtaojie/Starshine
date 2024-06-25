// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// UnitOfWork帮助类
/// </summary>
public static class UnitOfWorkHelper
{
    /// <summary>
    /// 获取UnitOfWorkAttribute
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    public static UnitOfWorkAttribute? GetUnitOfWorkAttribute(MethodInfo methodInfo)
    {
        var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
        if (attrs.Length > 0)
        {
            return attrs[0];
        }

        if (methodInfo.DeclaringType != null)
        {
            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }
        }

        return null;
    }

    private static bool AnyMethodHasUnitOfWorkAttribute(TypeInfo implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasUnitOfWorkAttribute);
    }

    private static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
    }
}
