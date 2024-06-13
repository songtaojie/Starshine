// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Common.DependencyInjection
{
    /// <summary>
    /// 设置依赖注入方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expectInterfaces"></param>
        public InjectionAttribute(params Type[] expectInterfaces)
        {
            Pattern = InjectionPatterns.FirstInterface;
            ExpectInterfaces = expectInterfaces ?? Array.Empty<Type>();
            Order = 0;
        }

        /// <summary>
        /// 注册选项，默认为FirstInterface
        /// </summary>
        public InjectionPatterns Pattern { get; set; }

        /// <summary>
        /// 注册别名
        /// </summary>
        /// <remarks>多服务时使用</remarks>
        public string Named { get; set; }

        /// <summary>
        /// 排序，排序越大，则在后面注册
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 排除接口
        /// </summary>
        public Type[] ExpectInterfaces { get; set; }
    }
}
