// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.DependencyInjection;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///使用.Net Core原生的依赖注入进行 依赖注入的拓展类
    /// </summary>
    public static class NativeDependencyInjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 使用.Net Core自带的DI添加依赖注入接口
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="types">依赖注入的类型集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddNativeDependencyInjection(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddScanDependencyInjection(types);
            return services;
        }
        /// <summary>
        /// 添加扫描注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="effectiveTypes"></param>
        /// <returns>服务集合</returns>
        private static IServiceCollection AddScanDependencyInjection(this IServiceCollection services, IEnumerable<Type> effectiveTypes)
        {
            var injectTypes = effectiveTypes
               .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract)
               .OrderBy(GetOrder);
            foreach (var targetType in injectTypes)
            {
                var dependencyAttribute = GetDependencyAttribute(targetType);
                var lifeTime = GetLifeTime(targetType, dependencyAttribute);
                if (lifeTime == null) continue;
                var exposedServiceTypes = targetType.GetCustomAttributes(true)
                    .OfType<IExposedServiceTypesProvider>()
                    .DefaultIfEmpty(new ExposeServicesAttribute
                    { 
                        Pattern = DependencyInjectionPattern.FirstInterface
                    })
                    .SelectMany(r => r.GetExposedServiceTypes(targetType))
                    .Distinct();
                var nullableServiceKeyList = exposedServiceTypes.Where(x => x.ServiceKey == null).ToList();
                foreach (var exposedServiceType in exposedServiceTypes)
                {
                    var allExposingServiceTypes = exposedServiceType.ServiceKey == null
                        ? nullableServiceKeyList
                        : exposedServiceTypes.Where(x => x.ServiceKey?.ToString() == exposedServiceType.ServiceKey?.ToString()).ToList();

                    var serviceDescriptor = CreateServiceDescriptor(targetType, exposedServiceType.ServiceKey, exposedServiceType.ServiceType, allExposingServiceTypes, lifeTime.Value);

                    if (dependencyAttribute?.Replace == true)
                    {
                        services.Replace(serviceDescriptor);
                    }
                    else if (dependencyAttribute?.TryAdd == false)
                    {
                        services.Add(serviceDescriptor);

                    }
                    else
                    {
                        services.TryAdd(serviceDescriptor);
                    }

                }
            }

            DescribeKeyedService(ServiceLifetime.Singleton);
            DescribeKeyedService(ServiceLifetime.Scoped);
            DescribeKeyedService(ServiceLifetime.Transient);
            return services;
        }

        private static ServiceDescriptor CreateServiceDescriptor(Type implementationType, object? serviceKey, Type exposingServiceType,
            List<ServiceIdentifier> allExposingServiceTypes,
            ServiceLifetime lifeTime)
        {
            var serviceLifetimeArr = new ServiceLifetime[] { ServiceLifetime.Singleton, ServiceLifetime.Scoped };
            if (serviceLifetimeArr.Contains(lifeTime))
            {
                var redirectedType = GetRedirectedType(implementationType, exposingServiceType, allExposingServiceTypes);

                if (redirectedType != null)
                {
                    if (serviceKey != null)
                    {
                        TypeKeyedCollection.TryAdd(serviceKey, redirectedType);
                    }
                    return ServiceDescriptor.Describe(
                            exposingServiceType,
                            provider => provider.GetService(redirectedType)!,
                            lifeTime
                        );
                }
            }
            if (serviceKey != null)
            {
                TypeKeyedCollection.TryAdd(serviceKey, implementationType);
            }
            return ServiceDescriptor.Describe(
                    exposingServiceType,
                    implementationType,
                    lifeTime
                );
        }

        private static Type? GetRedirectedType(
            Type implementationType,
            Type exposingServiceType,
            List<ServiceIdentifier> allExposingKeyedServiceTypes)
        {
            if (allExposingKeyedServiceTypes.Count < 2)
            {
                return null;
            }

            if (exposingServiceType == implementationType)
            {
                return null;
            }

            if (allExposingKeyedServiceTypes.Any(t => t.ServiceType == implementationType))
            {
                return implementationType;
            }

            return allExposingKeyedServiceTypes.FirstOrDefault(
                t => t.ServiceType != exposingServiceType && exposingServiceType.IsAssignableFrom(t.ServiceType)
            ).ServiceType;
        }


        /// <summary>
        /// 注册命名服务（接口多实现）
        /// </summary>
        private static ServiceDescriptor DescribeKeyedService(ServiceLifetime serviceLifetime)
        {
            // 注册命名服务
            return ServiceDescriptor.Describe(typeof(Func<string, object?>), provider =>
            {
                object? ResolveService(string serviceKey)
                {
                    var isRegister = TypeKeyedCollection.TryGetValue(serviceKey, out var serviceType);
                    return isRegister ? provider.GetService(serviceType!) : null;
                }
                return (Func<string, object?>)ResolveService;
            }, serviceLifetime);
        }

        /// <summary>
        /// 获取依赖注入属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static DependencyAttribute? GetDependencyAttribute(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true);
        }

        private static ServiceLifetime? GetLifeTime(Type type, DependencyAttribute? dependencyAttribute)
        {
            return dependencyAttribute?.Lifetime ?? GetServiceLifetimeFromClass(type);
        }

        private static ServiceLifetime? GetServiceLifetimeFromClass(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }


        /// <summary>
        /// 获取 注册 排序
        /// </summary>
        /// <param name="type">排序类型</param>
        /// <returns>int</returns>
        private static int GetOrder(Type type)
        {
            return type.GetCustomAttribute<DependencyAttribute>(true)?.Order ?? 0;
        }

        /// <summary>
        /// 类型名称集合
        /// </summary>
        private static readonly ConcurrentDictionary<object, Type> TypeKeyedCollection;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NativeDependencyInjectionServiceCollectionExtensions()
        {
            TypeKeyedCollection = new ConcurrentDictionary<object, Type>();
        }

    }
}