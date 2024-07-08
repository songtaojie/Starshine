// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Abstractions;

/// <summary>
/// ActionDescriptor扩展类
/// </summary>
public static class ActionDescriptorExtensions
{
    /// <summary>
    /// 转换为ControllerActionDescriptor对象
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
    {
        if (!actionDescriptor.IsControllerAction())
        {
            throw new Exception($"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
        }

        return (actionDescriptor as ControllerActionDescriptor)!;
    }

    /// <summary>
    /// 获取方法信息
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
    }

    /// <summary>
    /// 获取返回类型
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    public static Type GetReturnType(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor.GetMethodInfo().ReturnType;
    }

    /// <summary>
    /// 是否是ControllerActionDescriptor
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor is ControllerActionDescriptor;
    }

    /// <summary>
    /// 是否是PageActionDescriptor
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    public static bool IsPageAction(this ActionDescriptor actionDescriptor)
    {
        return actionDescriptor is PageActionDescriptor;
    }

    /// <summary>
    /// 转换为PageActionDescriptor
    /// </summary>
    /// <param name="actionDescriptor"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static PageActionDescriptor AsPageAction(this ActionDescriptor actionDescriptor)
    {
        if (!actionDescriptor.IsPageAction())
        {
            throw new Exception($"{nameof(actionDescriptor)} should be type of {typeof(PageActionDescriptor).AssemblyQualifiedName}");
        }

        return (actionDescriptor as PageActionDescriptor)!;
    }
}
