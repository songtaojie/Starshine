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

namespace Starshine.DependencyInjection;
/// <summary>
/// 获取暴漏服务的接口
/// </summary>
public interface IExposedServiceTypesProvider
{
    /// <summary>
    /// 获取所有暴漏的服务
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    List<ServiceIdentifier> GetExposedServiceTypes(Type targetType);
}
