// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Starshine.DependencyInjection;
/// <summary>
/// 依赖注入，暴露哪些服务
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExposeServicesAttribute:Attribute, IExposedServiceTypesProvider
{
    /// <summary>
    /// 暴漏的服务
    /// </summary>
    /// <param name="serviceKey">服务的key</param>
    /// <param name="serviceType">服务类型</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExposeServicesAttribute(object serviceKey, Type serviceType)
    {
        ServiceKey = serviceKey ?? throw new ArgumentNullException($"{nameof(serviceKey)} can not be null!");
        ServiceTypes = new Type[] { serviceType };
    }

    /// <summary>
    /// 暴漏的服务
    /// </summary>
    /// <param name="serviceTypes"></param>
    public ExposeServicesAttribute(params Type[] serviceTypes)
    {
        ServiceTypes = serviceTypes ?? Type.EmptyTypes;
    }

    /// <summary>
    /// 注册服务的模式，默认为FirstInterface
    /// </summary>
    public DependencyInjectionPattern Pattern { get; set; }

    /// <summary>
    /// 注册别名
    /// </summary>
    /// <remarks>多服务时使用</remarks>
    public object? ServiceKey { get; set; }

    /// <summary>
    /// 包含哪些类型
    /// </summary>
    public Type[] ServiceTypes { get; set; }

    /// <summary>
    /// 获取所有暴漏的服务
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public List<ServiceIdentifier> GetExposedServiceTypes(Type targetType)
    {
        var serviceList = ServiceTypes.Select(r=>new ServiceIdentifier(ServiceKey,r)).ToList();

        var canInjectInterfaces = targetType.GetTypeInfo().GetInterfaces().Where(u => u != typeof(IScopedDependency)
                              && u != typeof(ITransientDependency)
                              && u != typeof(ISingletonDependency)
                              && (
                                  (!targetType.IsGenericType && !u.IsGenericType)
                                  || (targetType.IsGenericType && u.IsGenericType && targetType.GetGenericArguments().Length == u.GetGenericArguments().Length)))
        .Select(FixedGenericType);
        if ((Pattern & DependencyInjectionPattern.AllInterfaces) == DependencyInjectionPattern.AllInterfaces)
        {
            foreach (var type in canInjectInterfaces)
            {
                if (type == null) continue;
                var erviceIdentifier = new ServiceIdentifier(ServiceKey, type);
                if (!serviceList.Contains(erviceIdentifier))
                {
                    serviceList.Add(erviceIdentifier);
                }
            }
        }
        if ((Pattern & DependencyInjectionPattern.Self) == DependencyInjectionPattern.Self)
        {
            var erviceIdentifier = new ServiceIdentifier(ServiceKey, targetType);
            if (!serviceList.Contains(erviceIdentifier))
            {
                serviceList.Add(erviceIdentifier);
            }
        }
        if ((Pattern & DependencyInjectionPattern.FirstInterface) == DependencyInjectionPattern.FirstInterface)
        {
            var firstInterfaces = canInjectInterfaces.FirstOrDefault(r=>r.Name.Contains(targetType.Name)) ?? canInjectInterfaces.FirstOrDefault();
            if (firstInterfaces != null)
            {
                var erviceIdentifier = new ServiceIdentifier(ServiceKey, targetType);
                if (!serviceList.Contains(erviceIdentifier))
                {
                    serviceList.Add(erviceIdentifier);
                }
            }
        }
        return serviceList;
    }

    /// <summary>
    /// 修复泛型类型注册类型问题
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns></returns>
    private static Type FixedGenericType(Type type)
    {
        if (!type.IsGenericType) return type;

        return type.Assembly.GetType($"{type.Namespace}.{type.Name}", true, true)!;
    }
}
