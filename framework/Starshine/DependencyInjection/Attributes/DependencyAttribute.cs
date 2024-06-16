// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using Starshine.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.DependencyInjection
{
    /// <summary>
    /// 设置依赖注入方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DependencyAttribute()
        {
            Order = 0;
            TryAdd = true;
        }

        /// <summary>
        /// 生命周期
        /// </summary>
        public ServiceLifetime? Lifetime { get; set; }

        /// <summary>
        /// 服务注册方式,
        /// true:如果存在则跳过，默认方式
        /// false:直接添加
        /// </summary>
        public bool TryAdd { get; set; }

        /// <summary>
        /// 替换服务
        /// </summary>
        public bool Replace { get; set; }

       
        /// <summary>
        /// 排序，排序越大，则在后面注册
        /// </summary>
        public int Order { get; set; }
    }
}
