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

namespace Microsoft.Extensions.DependencyInjection;
/// <summary>
/// UserManager扩展类
/// </summary>
public static class UserManagerServiceCollectionExtensions
{
    /// <summary>
    /// 添加UserManager,可以获取用户信息
    /// 使用时只需要在构造函数注入UserManager即可
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.AddHttpContextAccessor();
        services.AddScoped<UserManager>();
        return services;
    }
}
