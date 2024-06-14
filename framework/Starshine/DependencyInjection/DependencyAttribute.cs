// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine
{
    /// <summary>
    /// 设置依赖注入方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceTypes"></param>
        public DependencyAttribute(params Type[] serviceTypes)
        {
            Pattern = DependencyInjectionPatterns.FirstInterface;
            ServiceTypes = serviceTypes ?? Array.Empty<Type>();
            Order = 0;
            TryAdd = true;
        }

        /// <summary>
        /// 生命周期
        /// </summary>
        public virtual ServiceLifetime? Lifetime { get; set; }

        /// <summary>
        /// 服务注册方式,
        /// true:如果存在则跳过，默认方式
        /// false:直接添加
        /// </summary>
        public virtual bool TryAdd { get; set; }

        /// <summary>
        /// 替换服务
        /// </summary>
        public virtual bool Replace { get; set; }

        /// <summary>
        /// 注册选项，默认为FirstInterface
        /// </summary>
        public virtual DependencyInjectionPatterns Pattern { get; set; }

        /// <summary>
        /// 注册别名
        /// </summary>
        /// <remarks>多服务时使用</remarks>
        public virtual string? ServiceKey { get; set; }

        /// <summary>
        /// 排序，排序越大，则在后面注册
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// 包含哪些接口
        /// </summary>
        public virtual Type[] ServiceTypes { get; set; }

        public Type[] GetExposedServiceTypes(Type targetType)
        {
            var serviceList = ServiceTypes.ToList();
            if ((Pattern & DependencyInjectionPatterns.AllInterfaces) == DependencyInjectionPatterns.AllInterfaces)
            {
                var canInjectInterfaces = targetType.GetInterfaces()
                       .Where(u => u != typeof(IScopedDependency)
                                   && u != typeof(ITransientDependency)
                                   && u != typeof(ISingletonDependency)
                                   && (
                                       (!targetType.IsGenericType && !u.IsGenericType)
                                       || (targetType.IsGenericType && u.IsGenericType && targetType.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                   );
                foreach (var type in canInjectInterfaces)
                {
                    if (!serviceList.Contains(type))
                    {
                        serviceList.Add(type);
                    }
                }
            }
            if (IncludeDefaults)
            {
                foreach (var type in GetDefaultServices(targetType))
                {
                    serviceList.AddIfNotContains(type);
                }

                if (IncludeSelf)
                {
                    serviceList.AddIfNotContains(targetType);
                }
            }
            else if (IncludeSelf)
            {
                serviceList.AddIfNotContains(targetType);
            }

            return serviceList.ToArray();
        }

        private static List<Type> GetDefaultServices(Type type)
        {
            var serviceTypes = new List<Type>();
            var interfacesList = type.GetTypeInfo().GetInterfaces();
            foreach (var interfaceType in interfacesList)
            {
                var interfaceName = interfaceType.Name;
                if (interfaceType.IsGenericType)
                {
                    interfaceName = interfaceType.Name.Left(interfaceType.Name.IndexOf('`'));
                }

                if (interfaceName.StartsWith("I"))
                {
                    interfaceName = interfaceName.Right(interfaceName.Length - 1);
                }

                if (type.Name.EndsWith(interfaceName))
                {
                    serviceTypes.Add(interfaceType);
                }
            }

            return serviceTypes;
        }

    }
}
