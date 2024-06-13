using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace Hx.Sdk.Core
{
    /// <summary>
    /// Web管理类
    /// </summary>
    public sealed class WebManager
    {
        private readonly IHostEnvironment env;
        /// <summary>
        /// web帮助类
        /// </summary>
        /// <param name="env"></param>
        public WebManager(IHostEnvironment env)
        {
            this.env = env;
            var propInfo = env.GetType().GetProperty("WebRootPath");
            if (propInfo == null)
            {
                WebRootPath = Path.Combine(env.ContentRootPath, "wwwroot");
            }
            else
            {
                object rootPath = propInfo.GetValue(env);
                WebRootPath = rootPath?.ToString().Trim();
            }
            if (string.IsNullOrEmpty(WebRootPath)) throw new Exception("获取web应用程序根路径失败");
        }
        /// <summary>
        /// web应用程序根路径
        /// </summary>
        public string WebRootPath { get; }

        /// <summary>
        /// web应用程序根路径
        /// </summary>
        public string ContentRootPath => env.ContentRootPath;
        /// <summary>
        /// 把绝对路径转换成相对路径
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns></returns>
        public string ToRelativePath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath)) return string.Empty;
            return absolutePath.Replace(WebRootPath, "/").Replace(@"\", @"/"); //转换成相对路径
        }
        /// <summary>
        /// 把相对路径转换成绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public string ToAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return relativePath;
            if (relativePath.First() == '/')
            {
                relativePath = relativePath.Substring(1);
            }
            return Path.Combine(WebRootPath, relativePath);
        }

        /// <summary>
        /// 获取路由的全路径
        /// </summary>
        /// <param name="host">网站host</param>
        /// <param name="routeUrl"></param>
        /// <returns></returns>
        public string GetFullUrl(string host, string routeUrl)
        {
            if (string.IsNullOrEmpty(routeUrl)) return host;

            while (!string.IsNullOrEmpty(host) && host.Last() == '/')
            {
                host = host.Remove(host.Length - 1);
            }
            while (!string.IsNullOrEmpty(routeUrl) && routeUrl.First() == '/')
            {
                routeUrl = routeUrl.Substring(1);
            }
            return host + "/" + routeUrl;
        }
    }
}
