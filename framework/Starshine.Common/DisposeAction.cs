// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.Common
{
    /// <summary>
    ///  这个类可用于在调用Dispose方法时提供一个操作。
    /// </summary>
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        /// 创建 <see cref="DisposeAction"/> 对象.
        /// </summary>
        /// <param name="action">在此对象被处置时执行的操作。</param>
        public DisposeAction(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _action();
        }
    }

    /// <summary>
    /// 这个类可用于在调用Dispose方法时提供一个操作。
    /// <typeparam name="T">动作参数的类型。</typeparam>
    /// </summary>
    public class DisposeAction<T> : IDisposable
    {
        private readonly Action<T> _action;

        private readonly T _parameter;

        /// <summary>
        /// 创建 <see cref="DisposeAction"/> 对象.
        /// </summary>
        /// <param name="action">在此对象被处置时执行的操作</param>
        /// <param name="parameter">动作参数的类型</param>
        public DisposeAction(Action<T> action, T parameter)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _parameter = parameter;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_parameter != null)
            {
                _action(_parameter);
            }
        }
    }
}
