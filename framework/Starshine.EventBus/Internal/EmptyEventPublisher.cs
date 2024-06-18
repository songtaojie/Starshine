using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Starshine.EventBus.Internal
{
    /// <summary>
    /// 无效的事件总线
    /// </summary>
    internal class EmptyEventPublisher : IEventPublisher
    {

        private readonly ILogger<EmptyEventPublisher> _logger;
        public EmptyEventPublisher(ILogger<EmptyEventPublisher> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc cref="IEventPublisher.PublishAsync(IEventSource)"/>
        public async Task PublishAsync(IEventSource eventSource)
        {
            _logger.LogWarning($"EmptyEventBus PublishAsync eventSource:{JsonSerializer.Serialize(eventSource)}");
            await Task.CompletedTask;
        }

        /// <inheritdoc cref="IEventPublisher.PublishDelayAsync(IEventSource,long)"/>
        public async Task PublishDelayAsync(IEventSource eventSource, long delay)
        {
            _logger.LogWarning($"EmptyEventBus PublishDelayAsync eventSource:{JsonSerializer.Serialize(eventSource)}");
            await Task.CompletedTask;
        }

        /// <inheritdoc cref="IEventPublisher.PublishAsync(string,object,CancellationToken)"/>
        public async Task PublishAsync(string eventId, object? payload = default, CancellationToken cancellationToken = default)
        {
            await PublishAsync(new ChannelEventSource(eventId, payload, cancellationToken));
        }

        /// <inheritdoc cref="IEventPublisher.PublishAsync(Enum,object,CancellationToken)"/>
        public async Task PublishAsync(Enum eventId, object? payload = default, CancellationToken cancellationToken = default)
        {
            await PublishAsync(new ChannelEventSource(eventId, payload, cancellationToken));
        }

        /// <inheritdoc cref="IEventPublisher.PublishDelayAsync(string,long,object,CancellationToken)"/>
        public async Task PublishDelayAsync(string eventId, long delay, object? payload = default, CancellationToken cancellationToken = default)
        {
            await PublishDelayAsync(new ChannelEventSource(eventId, payload, cancellationToken), delay);
        }

        /// <inheritdoc cref="IEventPublisher.PublishDelayAsync(Enum,long,object,CancellationToken)"/>
        public async Task PublishDelayAsync(Enum eventId, long delay, object? payload = default, CancellationToken cancellationToken = default)
        {
            await PublishDelayAsync(new ChannelEventSource(eventId, payload, cancellationToken), delay);
        }
    }
}
