// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Text;

namespace Starshine.DependencyInjection
{
    /// <summary>
    /// 每个依赖一个实例(即每次都重新实例),使用每个依赖的作用域, 
    /// 当你解析一个每个依赖一个实例的组件时, 你每次获得一个新的实例.
    /// </summary>
    public interface ITransientDependency
    {
    }
}
