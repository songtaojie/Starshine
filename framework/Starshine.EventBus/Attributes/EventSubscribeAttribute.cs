using DotNetCore.CAP;
using System;

namespace Starshine.EventBus
{

    /// <summary>
    /// 事件处理程序特性
    /// </summary>
    /// <remarks>
    /// <para>作用于 <see cref="IEventSubscriber"/> 实现类实例方法</para>
    /// <para>支持多个事件 Id 触发同一个事件处理程序</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class EventSubscribeAttribute : CapSubscribeAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <remarks>只支持事件类型和 Enum 类型</remarks>
        public EventSubscribeAttribute(string eventId) : base(eventId)
        {
            EventId = eventId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <remarks>只支持事件类型和 Enum 类型</remarks>
        public EventSubscribeAttribute(Enum eventId) : base(eventId.ParseToString())
        {
            EventId = eventId.ParseToString();
        }

        /// <summary>
        /// 事件 Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 是否启用模糊匹配消息
        /// </summary>
        /// <remarks>支持正则表达式，bool 类型，默认为 null</remarks>
        public object? FuzzyMatch { get; set; }

        /// <summary>
        /// 是否启用执行完成触发 GC 回收
        /// </summary>
        /// <remarks>bool 类型，默认为 null</remarks>
        public object? GCCollect { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int NumRetries { get; set; } = 0;

        /// <summary>
        /// 重试间隔时间
        /// </summary>
        /// <remarks>默认1000毫秒</remarks>
        public TimeSpan[]? RetryTimeout { get; set; }

        /// <summary>
        /// 可以指定特定异常类型才重试
        /// </summary>
        public Type[]? ExceptionTypes { get; set; }

        /// <summary>
        /// 重试失败策略配置
        /// </summary>
        /// <remarks>如果没有注册，必须通过 options.AddFallbackPolicy(type) 注册</remarks>
        public Type? FallbackPolicy { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>数值越大的先执行</remarks>
        public int Order { get; set; } = 0;
    }
}
