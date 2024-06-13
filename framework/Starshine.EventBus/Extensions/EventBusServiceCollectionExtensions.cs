using DotNetCore.CAP;
using Starshine.EventBus;
using Starshine.EventBus.Internal;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// EventBus 模块服务拓展
    /// </summary>
    public static class EventBusServiceCollectionExtensions1
    {

        /// <summary>
        /// 添加 EventBus 模块注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config">配置</param>
        /// <param name="configureOptionsBuilder">事件总线配置选项构建器委托</param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration config, Action<EventBusOptionsBuilder> configureOptionsBuilder = default)
        {
            // 创建初始事件总线配置选项构建器
            var eventBusOptionsBuilder = EventBusOptionsBuilder.Init(config);
            configureOptionsBuilder.Invoke(eventBusOptionsBuilder);

            if (eventBusOptionsBuilder.IsEnabled == true)
            {
                if (eventBusOptionsBuilder.UseCap == true)
                {
                    return services.AddCapEventBus(config, eventBusOptionsBuilder.CapOptions);
                }
                else
                {
                    return services.AddEventBus(eventBusOptionsBuilder);
                }
            }
            else
            {
                return services.AddEmptyEventBus();
            }
        }

        /// <summary>
        /// 添加cap RabbitMq
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config">配置</param>
        /// <param name="capOptions">cap配置，如果没设置，需要配置EventBus:Cap</param>
        /// <returns></returns>
        public static IServiceCollection AddCapEventBus(this IServiceCollection services, IConfiguration config, Action<CapOptions> capOptions = null)
        {
            services.AddCap(options =>
            {
                EventBusOptionsBuilder.InitCapOptions(config).Invoke(options);
                capOptions?.Invoke(options);
            });
            services.AddTransient<IEventPublisher, CapEventPublisher>();
            return services;
        }


        /// <summary>
        /// 添加空实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmptyEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher, EmptyEventPublisher>();
            return services;
        }

        /// <summary>
        /// 添加 EventBus 模块注册
        /// </summary>
        /// <param name="services">服务集合对象</param>
        /// <param name="eventBusOptionsBuilder">事件总线配置选项构建器</param>
        /// <returns>服务集合实例</returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, EventBusOptionsBuilder eventBusOptionsBuilder = default)
        {
            // 初始化事件总线配置项
            eventBusOptionsBuilder ??= new EventBusOptionsBuilder();

            // 注册内部服务
            services.AddInternalService(eventBusOptionsBuilder);

            // 构建事件总线服务
            eventBusOptionsBuilder.Build(services);

            // 通过工厂模式创建
            services.AddHostedService(serviceProvider =>
            {
                // 创建事件总线后台服务对象
                var eventBusHostedService = ActivatorUtilities.CreateInstance<EventBusHostedService>(
                    serviceProvider
                    , eventBusOptionsBuilder.UseUtcTimestamp
                    , eventBusOptionsBuilder.FuzzyMatch
                    , eventBusOptionsBuilder.GCCollect
                    , eventBusOptionsBuilder.EnabledLog);

                // 订阅未察觉任务异常事件
                var unobservedTaskExceptionHandler = eventBusOptionsBuilder.UnobservedTaskExceptionHandler;
                if (unobservedTaskExceptionHandler != default)
                {
                    eventBusHostedService.UnobservedTaskException += unobservedTaskExceptionHandler;
                }

                return eventBusHostedService;
            });

            return services;
        }

        /// <summary>
        /// 注册内部服务
        /// </summary>
        /// <param name="services">服务集合对象</param>
        /// <param name="eventBusOptionsBuilder">事件总线配置选项构建器</param>
        /// <returns>服务集合实例</returns>
        private static IServiceCollection AddInternalService(this IServiceCollection services, EventBusOptionsBuilder eventBusOptionsBuilder)
        {
            // 创建默认内存通道事件源对象
            var defaultStorerOfChannel = new ChannelEventSourceStorer(eventBusOptionsBuilder.ChannelCapacity);

            // 注册后台任务队列接口/实例为单例，采用工厂方式创建
            services.AddSingleton<IEventSourceStorer>(_ =>
            {
                return defaultStorerOfChannel;
            });

            // 注册默认内存通道事件发布者
            services.AddSingleton<IEventPublisher, ChannelEventPublisher>();

            // 注册事件总线工厂
            services.AddSingleton<IEventBusFactory, EventBusFactory>();

            return services;
        }
    }
}
