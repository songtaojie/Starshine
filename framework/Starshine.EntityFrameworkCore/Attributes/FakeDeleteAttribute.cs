// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 假删除/软删除
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FakeDeleteAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="state"></param>
        public FakeDeleteAttribute(object state)
        {
            State = state;
        }

        /// <summary>
        /// 假删除/软删除状态
        /// </summary>
        public object State { get; set; }
    }
}