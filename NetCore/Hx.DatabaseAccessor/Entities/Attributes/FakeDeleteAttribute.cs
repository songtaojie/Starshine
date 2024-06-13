﻿using System;

namespace Hx.DatabaseAccessor
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