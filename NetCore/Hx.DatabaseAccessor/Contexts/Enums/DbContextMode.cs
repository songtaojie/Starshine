﻿using System.ComponentModel;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库上下文模式
    /// </summary>
    public enum DbContextMode
    {
        /// <summary>
        /// 缓存模型数据库上下文
        /// <para>
        /// OnModelCreating 只会初始化一次
        /// </para>
        /// </summary>
        [Description("缓存模型数据库上下文")]
        Cached,

        /// <summary>
        /// 动态模型数据库上下文
        /// <para>
        /// OnModelCreating 每次都会调用
        /// </para>
        /// </summary>
        [Description("动态模型数据库上下文")]
        Dynamic
    }
}