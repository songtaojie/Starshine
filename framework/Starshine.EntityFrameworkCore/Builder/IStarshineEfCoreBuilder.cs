// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// StarshineEfCore构建器
/// </summary>
public interface IStarshineEfCoreBuilder
{
    /// <summary>
    /// 服务
    /// </summary>
    IServiceCollection Services { get; }
}
