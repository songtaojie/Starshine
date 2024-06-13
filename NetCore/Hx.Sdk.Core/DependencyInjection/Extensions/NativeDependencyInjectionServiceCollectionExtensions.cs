// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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
            // 查找所有需要依赖注入的类型
            var injectTypes = effectiveTypes
                .Where(u => typeof(IScopedDependency).IsAssignableFrom(u) && u.IsClass && !u.IsInterface && !u.IsAbstract)
                .OrderBy(u => GetOrder(u));
            var scopedTypes = effectiveTypes
               .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && (typeof(IScopedDependency).IsAssignableFrom(u) || u.IsDefined(typeof(ScopedDependencyAttribute))))
               .OrderBy(u => GetOrder(u));
            var transientTypes = effectiveTypes
              .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && (typeof(ITransientDependency).IsAssignableFrom(u) || u.IsDefined(typeof(TransientDependencyAttribute))))
              .OrderBy(u => GetOrder(u));
            var singletonTypes = effectiveTypes
              .Where(u => u.IsClass && !u.IsInterface && !u.IsAbstract && (typeof(ISingletonDependency).IsAssignableFrom(u) || u.IsDefined(typeof(SingletonDependencyAttribute))))
              .OrderBy(u => GetOrder(u));
            if (scopedTypes.Any())
            {
                foreach (var type in scopedTypes)
                {
                    // 获取注册方式
                    var injectionAttribute = !type.IsDefined(typeof(InjectionAttribute)) ? new InjectionAttribute() : type.GetCustomAttribute<InjectionAttribute>();
                    // 获取所有能注册的接口
                    var canInjectInterfaces = type.GetInterfaces()
                        .Where(u => !injectionAttribute.ExpectInterfaces.Contains(u)
                                    && u != typeof(IScopedDependency)
                                    && u != typeof(ITransientDependency)
                                    && u != typeof(ISingletonDependency)
                                    && (
                                        (!type.IsGenericType && !u.IsGenericType)
                                        || (type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                    );
                    // 注册服务
                    RegisterService(services, type, injectionAttribute, canInjectInterfaces, DependencyInjectionType.Scoped);
                    // 缓存类型注册
                    var typeNamed = injectionAttribute.Named ?? type.FullName;
                    TypeNamedCollection.TryAdd(typeNamed, type);
                }
            }
            if (transientTypes.Any())
            {
                foreach (var type in transientTypes)
                {
                    // 获取注册方式
                    var injectionAttribute = !type.IsDefined(typeof(InjectionAttribute)) ? new InjectionAttribute() : type.GetCustomAttribute<InjectionAttribute>();
                    // 获取所有能注册的接口
                    var canInjectInterfaces = type.GetInterfaces()
                        .Where(u => !injectionAttribute.ExpectInterfaces.Contains(u)
                                    && u != typeof(IScopedDependency)
                                    && u != typeof(ITransientDependency)
                                    && u != typeof(ISingletonDependency)
                                    && (
                                        (!type.IsGenericType && !u.IsGenericType)
                                        || (type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                    );
                    // 注册服务
                    RegisterService(services, type, injectionAttribute, canInjectInterfaces, DependencyInjectionType.Transient);
                    // 缓存类型注册
                    var typeNamed = injectionAttribute.Named ?? type.FullName;
                    TypeNamedCollection.TryAdd(typeNamed, type);
                }
            }

            if (singletonTypes.Any())
            {
                foreach (var type in singletonTypes)
                {
                    // 获取注册方式
                    var injectionAttribute = !type.IsDefined(typeof(InjectionAttribute)) ? new InjectionAttribute() : type.GetCustomAttribute<InjectionAttribute>();
                    // 获取所有能注册的接口
                    var canInjectInterfaces = type.GetInterfaces()
                        .Where(u => !injectionAttribute.ExpectInterfaces.Contains(u)
                                    && u != typeof(IScopedDependency)
                                    && u != typeof(ITransientDependency)
                                    && u != typeof(ISingletonDependency)
                                    && (
                                        (!type.IsGenericType && !u.IsGenericType)
                                        || (type.IsGenericType && u.IsGenericType && type.GetGenericArguments().Length == u.GetGenericArguments().Length))
                                    );
                    // 注册服务
                    RegisterService(services, type, injectionAttribute, canInjectInterfaces, DependencyInjectionType.Singleton);
                    // 缓存类型注册
                    var typeNamed = injectionAttribute.Named ?? type.FullName;
                    TypeNamedCollection.TryAdd(typeNamed, type);
                }
            }

            // 注册命名服务
            RegisterNamed(services);

            return services;
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="type">类型</param>
        /// <param name="injectionAttribute">注入特性</param>
        /// <param name="canInjectInterfaces">能被注册的接口</param>
        /// <param name="registerType"></param>
        private static void RegisterService(IServiceCollection services, Type type, 
                InjectionAttribute injectionAttribute, 
                IEnumerable<Type> canInjectInterfaces, 
                DependencyInjectionType registerType)
        {
            // 注册自己
            if (injectionAttribute.Pattern is InjectionPatterns.Self)
            {
                RegisterType(services, registerType, type);
            }

            if (!canInjectInterfaces.Any()) return;

            // 只注册第一个接口
            if (injectionAttribute.Pattern is InjectionPatterns.FirstInterface)
            {
                RegisterType(services, registerType, type, canInjectInterfaces.First());
            }
            // 注册多个接口
            else if (injectionAttribute.Pattern is InjectionPatterns.ImplementedInterfaces)
            {
                foreach (var inter in canInjectInterfaces)
                {
                    RegisterType(services, registerType, type,inter);
                }
            }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="registerType">注册类型</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterType(IServiceCollection services, DependencyInjectionType registerType, Type type, Type inter = null)
        {
            // 修复泛型注册类型
            var fixedType = FixedGenericType(type);
            var fixedInter = inter == null ? null : FixedGenericType(inter);
            if (registerType == DependencyInjectionType.Transient) RegisterTransientType(services, fixedType, fixedInter);
            if (registerType == DependencyInjectionType.Scoped) RegisterScopeType(services, fixedType, fixedInter);
            if (registerType == DependencyInjectionType.Singleton) RegisterSingletonType(services, fixedType, fixedInter);
        }

        /// <summary>
        /// 注册瞬时接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterTransientType(IServiceCollection services, Type type, Type inter = null)
        {
            if (inter == null) services.AddTransient(type);
            else
            {
                services.AddTransient(inter, type);
            }
        }

        /// <summary>
        /// 注册作用域接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterScopeType(IServiceCollection services, Type type,Type inter = null)
        {
            if (inter == null) services.AddScoped(type);
            else
            {
                services.AddScoped(inter, type);
            }
        }

        /// <summary>
        /// 注册单例接口实例类型
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="type">类型</param>
        /// <param name="inter">接口</param>
        private static void RegisterSingletonType(IServiceCollection services, Type type, Type inter = null)
        {
            if (inter == null) services.AddSingleton(type);
            else
            {
                services.AddSingleton(inter, type);
            }
        }

        /// <summary>
        /// 注册命名服务
        /// </summary>
        /// <param name="services">服务集合</param>
        private static void RegisterNamed(IServiceCollection services)
        {
            // 注册暂时命名服务
            services.AddTransient(provider =>
            {
                object ResolveService(string named, ITransientDependency transient)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, ITransientDependency, object>)ResolveService;
            });

            // 注册作用域命名服务
            services.AddScoped(provider =>
            {
                object ResolveService(string named, IScopedDependency scoped)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, IScopedDependency, object>)ResolveService;
            });

            // 注册单例命名服务
            services.AddSingleton(provider =>
            {
                object ResolveService(string named, ISingletonDependency singleton)
                {
                    var isRegister = TypeNamedCollection.TryGetValue(named, out var serviceType);
                    return isRegister ? provider.GetService(serviceType) : null;
                }
                return (Func<string, ISingletonDependency, object>)ResolveService;
            });
        }

        /// <summary>
        /// 修复泛型类型注册类型问题
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static Type FixedGenericType(Type type)
        {
            if (!type.IsGenericType) return type;

            return type.Assembly.GetType($"{type.Namespace}.{type.Name}", true, true);
        }

        /// <summary>
        /// 获取 注册 排序
        /// </summary>
        /// <param name="type">排序类型</param>
        /// <returns>int</returns>
        private static bool GetDependencyType(Type type)
        {
            return type.IsClass && !type.IsInterface && !type.IsAbstract 
                && (typeof(IScopedDependency).IsAssignableFrom(type) || typeof(ITransientDependency).IsAssignableFrom(type) || typeof(ISingletonDependency).IsAssignableFrom(type));
        }

        /// <summary>
        /// 获取 注册 排序
        /// </summary>
        /// <param name="type">排序类型</param>
        /// <returns>int</returns>
        private static int GetOrder(Type type)
        {
            return !type.IsDefined(typeof(InjectionAttribute), true)
                ? 0
                : type.GetCustomAttribute<InjectionAttribute>(true).Order;
        }

        /// <summary>
        /// 类型名称集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, Type> TypeNamedCollection;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NativeDependencyInjectionServiceCollectionExtensions()
        {
            TypeNamedCollection = new ConcurrentDictionary<string, Type>();
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        private enum DependencyInjectionType
        {
            /// <summary>
            /// 瞬时
            /// </summary>
            [Description("瞬时")]
            Transient,

            /// <summary>
            /// 作用域
            /// </summary>
            [Description("作用域")]
            Scoped,

            /// <summary>
            /// 单例
            /// </summary>
            [Description("单例")]
            Singleton
        }
    }
}