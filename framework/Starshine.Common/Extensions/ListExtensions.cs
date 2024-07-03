// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Extensions
{
    /// <summary>
    /// List扩展类
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// list异步便利
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }

        /// <summary>
        /// IEnumerable便利
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var value in source)
            {
                action(value);
            }
        }

        /// <summary>
        /// IEnumerable异步便利
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var value in source)
            {
                await action(value);
            }
        }
    }
}
