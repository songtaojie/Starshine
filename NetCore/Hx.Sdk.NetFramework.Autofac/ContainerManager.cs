using Autofac;
using Hx.Sdk.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hx.Sdk.NetFramework.Autofac
{
    /// <summary>
    /// 依赖注入管理类
    /// </summary>
    public class ContainerManager
    {
        private static ContainerBuilder _builder;

        /// <summary>
        /// 获取当前容器的builder
        /// </summary>
        public static ContainerBuilder Builder
        {
            get
            {
                if (_builder == null) _builder = new ContainerBuilder();
                return _builder;
            }
        }

        /// <summary>
        /// 创建，连接依赖关系并管理一组组件的生命周期的容器
        /// </summary>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
        /// <summary>
        /// 从上下文中检索服务。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        /// <summary>
        /// 在应用程序启动时进行服务的注入
        /// 如果要每次获取一个新实例，则类需要继承ITransientDependency
        /// 如果要注入单例，则类需要继承ISingletonDependency
        /// 如果同一个请求获取一个实例，则类需要继承IScopedDependency
        /// </summary>
        /// <param name="assemblyList">要注入的程序集</param>
        /// <param name="beforeBuild">一个委托在Build之前会执行当前委托</param>
        public void Build(IEnumerable<Assembly> assemblyList, Action<ContainerBuilder> beforeBuild = null)
        {
            if (assemblyList != null && assemblyList.Count() >= 0)
            {
                Type tdType = typeof(ITransientDependency);
                // 获取所有相关类库的程序集
                var assemblies = assemblyList.ToArray();
                Builder.RegisterAssemblyTypes(assemblies).Where(type => tdType.IsAssignableFrom(type) && !type.IsAbstract)
                    .AsImplementedInterfaces().InstancePerLifetimeScope();//每次解析获得新实例

                Type singletonType = typeof(ISingletonDependency);
                Builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                    .AsImplementedInterfaces().SingleInstance();// 保证对象生命周期基于单例

                Type requestType = typeof(IScopedDependency);
                Builder.RegisterAssemblyTypes(assemblies).Where(type => singletonType.IsAssignableFrom(type) && !type.IsAbstract)
                    .AsImplementedInterfaces().InstancePerRequest();// 保证对象生命周期基于单例
            }
            beforeBuild?.Invoke(Builder);
            Container = Builder.Build();
        }

        /// <summary>
        /// 在应用程序启动时进行服务的注入，对于web应用程序使用该方法，可以直接进行autofac的初始化
        /// 如果要每次获取一个新实例，则类需要继承ITransientDependency
        /// 如果要注入单例，则类需要继承ISingletonDependency
        /// 如果同一个请求获取一个实例，则类需要继承IScopedDependency
        /// </summary>
        public void BuildWeb(Action<ContainerBuilder> beforeBuild = null)
        {
            Assembly[] assemblies = RuntimeHelper.GetAllAssembly().ToArray();
            Build(assemblies, beforeBuild);
        }
    }
}
