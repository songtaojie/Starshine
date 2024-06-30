// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 事务api容器
/// </summary>
public interface ITransactionApiContainer
{
    /// <summary>
    /// 获取事务控制api
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ITransactionApi? FindTransactionApi([NotNull] string key);

    /// <summary>
    /// 添加事务控制api
    /// </summary>
    /// <param name="key"></param>
    /// <param name="api"></param>
    void AddTransactionApi([NotNull] string key, [NotNull] ITransactionApi api);

    /// <summary>
    /// 获取并添加事务控制api
    /// </summary>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    ITransactionApi GetOrAddTransactionApi([NotNull] string key, [NotNull] Func<ITransactionApi> factory);
}
