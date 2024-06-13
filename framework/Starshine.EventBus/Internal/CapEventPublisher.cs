using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hx.EventBus.Internal
{
    /// <summary>
    /// 基于Cap的发布实现
    /// </summary>
    internal sealed class CapEventPublisher : IEventPublisher
    {
        /// <summary>
        /// Cap发布
        /// </summary>
        private readonly ICapPublisher _capPublisher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventSourceStorer">事件源存储器</param>
        public CapEventPublisher(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 发布一条消息
        /// </summary>
        /// <param name="eventSource">事件源</param>
        /// <returns><see cref="Task"/> 实例</returns>
        public async Task PublishAsync(IEventSource eventSource)
        {
           await _capPublisher.PublishAsync(eventSource.EventId, eventSource.Payload, string.Empty, eventSource.CancellationToken);
        }

        /// <summary>
        /// 延迟发布一条消息
        /// </summary>
        /// <param name="eventSource">事件源</param>
        /// <param name="delay">延迟数（毫秒）</param>
        /// <returns><see cref="Task"/> 实例</returns>
        public async Task PublishDelayAsync(IEventSource eventSource, long delay)
        {
            await _capPublisher.PublishDelayAsync(TimeSpan.FromMilliseconds(delay),eventSource.EventId, eventSource.Payload,string.Empty, eventSource.CancellationToken);
        }

        /// <summary>
        /// 发布一条消息
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <param name="payload">事件承载（携带）数据</param>
        /// <param name="cancellationToken"> 取消任务 Token</param>
        /// <returns></returns>
        public async Task PublishAsync(string eventId, object payload = default, CancellationToken cancellationToken = default)
        {
            await PublishAsync(new ChannelEventSource(eventId, payload, cancellationToken));
        }

        /// <summary>
        /// 发布一条消息
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <param name="payload">事件承载（携带）数据</param>
        /// <param name="cancellationToken"> 取消任务 Token</param>
        /// <returns></returns>
        public async Task PublishAsync(Enum eventId, object payload = default, CancellationToken cancellationToken = default)
        {
            await PublishAsync(new ChannelEventSource(eventId, payload, cancellationToken));
        }

        /// <summary>
        /// 延迟发布一条消息
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <param name="delay">延迟数（毫秒）</param>
        /// <param name="payload">事件承载（携带）数据</param>
        /// <param name="cancellationToken"> 取消任务 Token</param>
        /// <returns><see cref="Task"/> 实例</returns>
        public async Task PublishDelayAsync(string eventId, long delay, object payload = default, CancellationToken cancellationToken = default)
        {
            await PublishDelayAsync(new ChannelEventSource(eventId, payload, cancellationToken), delay);
        }

        /// <summary>
        /// 延迟发布一条消息
        /// </summary>
        /// <param name="eventId">事件 Id</param>
        /// <param name="delay">延迟数（毫秒）</param>
        /// <param name="payload">事件承载（携带）数据</param>
        /// <param name="cancellationToken"> 取消任务 Token</param>
        /// <returns><see cref="Task"/> 实例</returns>
        public async Task PublishDelayAsync(Enum eventId, long delay, object payload = default, CancellationToken cancellationToken = default)
        {
            await PublishDelayAsync(new ChannelEventSource(eventId, payload, cancellationToken), delay);
        }
    }
}
