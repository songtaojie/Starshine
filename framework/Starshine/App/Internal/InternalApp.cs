﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Starshine.Internal
{
    /// <summary>
    /// 内部 App 副本
    /// </summary>
    [SkipScan]
    internal class InternalApp
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        internal static IServiceCollection? InternalServices { get; private set; }

        /// <summary>
        /// 设置服务
        /// </summary>
        /// <param name="services"></param>
        internal static void SetServiceCollection(IServiceCollection services)
        {
            InternalServices = services;
        }
       

        /// <summary>
        /// 配置对象
        /// </summary>
        internal static IConfiguration? Configuration { get; private set; }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="configuration"></param>
        internal static void SetConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// 获取Web主机环境
        /// </summary>
        internal static IWebHostEnvironment? WebHostEnvironment {get; private set; }
        /// <summary>
        /// 设置Web主机环境
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        internal static void SetWebHostEnvironment(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 获取泛型主机环境
        /// </summary>
        internal static IHostEnvironment? HostEnvironment;
        /// <summary>
        /// 设置泛型主机环境
        /// </summary>
        /// <param name="hostEnvironment"></param>
        internal static void SetHostEnvironment(IHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// 服务提供器
        /// </summary>
        internal static IServiceProvider? RootServices;

        /// <summary>
        /// 设置服务提供器
        /// </summary>
        /// <param name="provider"></param>
        internal static void SetHostEnvironment(IServiceProvider provider)
        {
            RootServices = provider;
        }

        /// <summary>
        /// 添加配置文件
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="hostEnvironment"></param>
        internal static void AddConfigureFiles(IConfigurationBuilder configurationBuilder, IHostEnvironment hostEnvironment)
        {
            var configuration = configurationBuilder is ConfigurationManager
                   ? (configurationBuilder as ConfigurationManager)!
                   : configurationBuilder.Build();
            // 获取程序执行目录
            var executeDirectory = AppContext.BaseDirectory;

            // 获取自定义配置扫描目录
            var configurationScanDirectories = (configuration.GetSection("ConfigurationScanDirectories")
                    .Get<string[]>()
                ?? Array.Empty<string>()).Select(u => Path.Combine(executeDirectory, u));
            // 扫描执行目录及自定义配置目录下的 *.json 文件
            var jsonFiles = new[] { executeDirectory }
                                .Concat(configurationScanDirectories)
                                .SelectMany(u =>
                                    Directory.GetFiles(u, "*.json", SearchOption.TopDirectoryOnly));
            // 获取环境变量名，如果没找到，则读取 NETCORE_ENVIRONMENT 环境变量信息识别（用于非 Web 环境）
            var envName = hostEnvironment?.EnvironmentName ?? Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Unknown";
            // 读取忽略的配置文件
            var ignoreConfigurationFiles = (configuration.GetSection("IgnoreConfigurationFiles")
                    .Get<string[]>()
                ?? Array.Empty<string>());
            // 处理控制台应用程序
            var _excludeJsonPrefixs = hostEnvironment == default ? excludeJsonPrefixs.Where(u => !u.Equals("appsettings")) : excludeJsonPrefixs;
            // 将所有文件进行分组
            var jsonFilesGroups = SplitConfigFileNameToGroups(jsonFiles)
                                        .Where(u => !_excludeJsonPrefixs.Contains(u.Key, StringComparer.OrdinalIgnoreCase) && !u.Any(c => runtimeJsonSuffixs.Any(z => c.EndsWith(z, StringComparison.OrdinalIgnoreCase)) || ignoreConfigurationFiles.Contains(Path.GetFileName(c), StringComparer.OrdinalIgnoreCase) || ignoreConfigurationFiles.Any(i => new Matcher().AddInclude(i).Match(Path.GetFileName(c)).HasMatches)));

            // 遍历所有配置分组
            foreach (var group in jsonFilesGroups)
            {
                // 限制查找的 json 文件组
                var limitFileNames = new[] { $"{group.Key}.json", $"{group.Key}.{envName}.json" };

                // 查找默认配置和环境配置
                var files = group.Where(u => limitFileNames.Contains(Path.GetFileName(u), StringComparer.OrdinalIgnoreCase))
                                                 .OrderBy(u => Path.GetFileName(u).Length);

                // 循环加载
                foreach (var jsonFile in files)
                {
                    configurationBuilder.AddJsonFile(jsonFile, optional: true, reloadOnChange: true);
                }
            }
        }

        /// <summary>
        /// 排除特定配置文件正则表达式
        /// </summary>
        private const string excludeJsonPattern = @"^{0}(\.\w+)?\.((json)|(xml))$";

        /// <summary>
        /// 排序的配置文件前缀
        /// </summary>
        private static readonly string[] excludeJsonPrefixs = new[] { "appsettings", "bundleconfig", "compilerconfig" };

        /// <summary>
        /// 排除运行时 Json 后缀
        /// </summary>
        private static readonly string[] runtimeJsonSuffixs = new[]
        {
            "deps.json",
            "runtimeconfig.dev.json",
            "runtimeconfig.prod.json",
            "runtimeconfig.json"
        };


        /// <summary>
        /// 对配置文件名进行分组
        /// </summary>
        /// <param name="configFiles"></param>
        /// <returns></returns>
        private static IEnumerable<IGrouping<string, string>> SplitConfigFileNameToGroups(IEnumerable<string> configFiles)
        {
            // 分组
            return configFiles.GroupBy(Function);

            // 本地函数
            static string Function(string file)
            {
                // 根据 . 分隔
                var fileNameParts = Path.GetFileName(file).Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (fileNameParts.Length == 2) return fileNameParts[0];

                return string.Join('.', fileNameParts.Take(fileNameParts.Length - 2));
            }
        }
    }
}
