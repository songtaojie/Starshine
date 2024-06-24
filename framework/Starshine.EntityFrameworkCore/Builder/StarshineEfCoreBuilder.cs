// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
internal sealed class StarshineEfCoreBuilder: IStarshineEfCoreBuilder
{
    /// <summary>
    ///初始化 <see cref="StarshineEfCoreBuilder"/> 实例.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" /> 添加服务</param>
    public StarshineEfCoreBuilder(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        Services = services;
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }
}
