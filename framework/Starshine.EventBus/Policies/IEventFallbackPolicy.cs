﻿
using System.Threading.Tasks;
using System;

namespace Hx.EventBus
{
    /// <summary>
    /// 事件重试失败回调服务
    /// </summary>
    /// <remarks>需注册为单例</remarks>
    public interface IEventFallbackPolicy
    {
        /// <summary>
        /// 重试失败回调
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        Task CallbackAsync(EventHandlerExecutingContext context, Exception ex);
    }
}

