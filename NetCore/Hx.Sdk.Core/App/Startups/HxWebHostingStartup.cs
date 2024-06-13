using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Hx.Sdk.Core.HxWebHostingStartup))]

namespace Hx.Sdk.Core
{
    /// <summary>
    /// 配置程序启动时自动注入
    /// </summary>
    [SkipScan]
    internal sealed class HxWebHostingStartup : IHostingStartup
    {
        /// <summary>
        /// 配置应用启动
        /// </summary>
        /// <param name="webHostBuilder"></param>
        public void Configure(IWebHostBuilder webHostBuilder)
        {
            // 自动装载配置
            webHostBuilder.ConfigureHxWebAppConfiguration();
        }
    }
}