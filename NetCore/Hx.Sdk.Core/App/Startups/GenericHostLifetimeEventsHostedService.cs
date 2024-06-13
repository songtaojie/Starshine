using Hx.Sdk.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// 监听泛型主机启动事件
    /// </summary>
    internal class GenericHostLifetimeEventsHostedService : IHostedService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        public GenericHostLifetimeEventsHostedService(IHost host)
        {
            // 存储服务提供器
            InternalApp.RootServices = host.Services;
        }

        /// <summary>
        /// 监听主机启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 监听主机停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
