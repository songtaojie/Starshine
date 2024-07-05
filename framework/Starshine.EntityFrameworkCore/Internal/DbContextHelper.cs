// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Profiling;
using System.Collections.Concurrent;

namespace Starshine.EntityFrameworkCore.Internal
{
    /// <summary>
    /// 常量、公共方法配置类
    /// </summary>
    internal static class DbContextHelper
    {

        /// <summary>
        /// 数据库上下文描述器
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Type> DbContextProviders;

        /// <summary>
        /// 构造函数
        /// </summary>
        static DbContextHelper()
        {
            DbContextProviders = new ConcurrentDictionary<Type, Type>();
        }

        /// <summary>
        /// 配置 SqlServer 数据库上下文
        /// </summary>
        /// <param name="optionBuilder">数据库上下文选项构建器</param>
        /// <param name="interceptors">拦截器</param>
        /// <returns></returns>
        internal static Action<IServiceProvider, DbContextOptionsBuilder> ConfigureDbContext(Action<IServiceProvider,DbContextOptionsBuilder> optionBuilder, params IInterceptor[] interceptors)
        {
            return (provider, options) =>
            {
                var dbSettingsOptions = provider.GetRequiredService<IOptionsMonitor<DbSettingsOptions>>();
                if (dbSettingsOptions.CurrentValue.EnabledSqlLog == true)
                {
                    options.EnableDetailedErrors()
                                .EnableSensitiveDataLogging();
                }

                optionBuilder.Invoke(provider,options);
                // 添加拦截器
                AddInterceptors(interceptors, options, dbSettingsOptions);
                options.UseApplicationServiceProvider(provider);
                //options.UseInternalServiceProvider(provider);
            };
        }

        /// <summary>
        /// 数据库数据库拦截器
        /// </summary>
        /// <param name="interceptors">拦截器</param>
        /// <param name="options"></param>
        /// <param name="dbSettingsOptions">db配置</param>
        private static void AddInterceptors(IInterceptor[] interceptors, DbContextOptionsBuilder options, IOptionsMonitor<DbSettingsOptions> dbSettingsOptions)
        {
            // 添加拦截器
            var interceptorList = DbProvider.GetDefaultInterceptors();

            if (dbSettingsOptions.CurrentValue.EnabledMiniProfiler == true)
            {
                interceptorList.Add(new SqlConnectionProfilerInterceptor(dbSettingsOptions));
            }
            if (interceptors != null && interceptors.Length > 0)
            {
                interceptorList.AddRange(interceptors);
            }
            options.AddInterceptors(interceptorList.ToArray());
        }

        /// <summary>
        /// 检查数据库上下文是否绑定
        /// </summary>
        /// <param name="dbContextLocatorType"></param>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        internal static void CheckDbContextLocator(Type dbContextLocatorType, out Type dbContextType)
        {
            if (!DbContextProviders.TryGetValue(dbContextLocatorType, out Type? cacheDbContextType)) 
                throw new InvalidCastException($" The dbcontext locator `{dbContextLocatorType.Name}` is not bind.");
            dbContextType = cacheDbContextType;
        }


        /// <summary>
        /// 打印验证信息到 MiniProfiler
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="isError">是否为警告消息</param>
        public static void PrintToMiniProfiler(string category, string state, string? message = null, bool isError = false)
        {
            // 打印消息
            string titleCaseategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
            var customTiming = MiniProfiler.Current.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseategory} {state}" : message, state);
            if (customTiming == null) return;

            // 判断是否是警告消息
            if (isError) customTiming.Errored = true;
        }
       
    }
}
