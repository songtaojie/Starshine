﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.EventBus
{
    /// <summary>
    /// 事件总线订阅管理事件源
    /// </summary>
    internal sealed class EventSubscribeOperateSource : IEventSource
    {
        /// <summary>
        /// 事件 Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 事件承载（携带）数据
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// 事件创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 取消任务 Token
        /// </summary>
        /// <remarks>用于取消本次消息处理</remarks>
        [System.Text.Json.Serialization.JsonIgnore]
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 事件处理程序
        /// </summary>
        internal Func<EventHandlerExecutingContext, Task> Handler { get; set; }

        /// <summary>
        /// 订阅特性
        /// </summary>
        internal EventSubscribeAttribute Attribute { get; set; }

        /// <summary>
        /// 触发的方法
        /// </summary>
        internal MethodInfo HandlerMethod { get; set; }

        /// <summary>
        /// 实际事件 Id
        /// </summary>
        internal string SubscribeEventId { get; set; }

        /// <summary>
        /// 事件订阅器操作选项
        /// </summary>
        internal EventSubscribeOperates Operate { get; set; }
    }
}

