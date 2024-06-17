// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.DependencyInjection;
/// <summary>
/// 服务定义
/// </summary>
public readonly struct ServiceIdentifier : IEquatable<ServiceIdentifier>
{
    /// <summary>
    /// 服务的键
    /// </summary>
    public object? ServiceKey { get; }

    /// <summary>
    /// 服务的类型
    /// </summary>
    public Type ServiceType { get; }

    /// <summary>
    /// 初始化服务
    /// </summary>
    /// <param name="serviceType">服务类型</param>
    public ServiceIdentifier(Type serviceType):this(null, serviceType)
    {
    }

    /// <summary>
    /// 初始化服务
    /// </summary>
    /// <param name="serviceKey">服务的键</param>
    /// <param name="serviceType">服务类型</param>
    public ServiceIdentifier(object? serviceKey, Type serviceType)
    {
        ServiceKey = serviceKey;
        ServiceType = serviceType;
    }
    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ServiceIdentifier other)
    {
        if (ServiceKey == null && other.ServiceKey == null)
        {
            return ServiceType == other.ServiceType;
        }
        else if (ServiceKey != null && other.ServiceKey != null)
        {
            return ServiceType == other.ServiceType && ServiceKey.Equals(other.ServiceKey);
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is ServiceIdentifier && Equals((ServiceIdentifier)obj);
    }
    /// <summary>
    /// 返回此实例的哈希码。
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        if (ServiceKey == null)
        {
            return ServiceType.GetHashCode();
        }
        unchecked
        {
            return (ServiceType.GetHashCode() * 397) ^ ServiceKey.GetHashCode();
        }
    }

    /// <summary>
    /// 获取指示此对象是否表示构造的泛型类型的值
    /// </summary>
    public bool IsConstructedGenericType => ServiceType.IsConstructedGenericType;

    /// <summary>
    /// 泛型定义
    /// </summary>
    /// <returns></returns>
    public ServiceIdentifier GetGenericTypeDefinition() => new ServiceIdentifier(ServiceKey, ServiceType.GetGenericTypeDefinition());

    /// <summary>
    /// 转字符串
    /// </summary>
    /// <returns></returns>
    public override string? ToString()
    {
        if (ServiceKey == null)
        {
            return ServiceType.ToString();
        }

        return $"({ServiceKey}, {ServiceType})";
    }
}
