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
/// DatabaseApi容器
/// </summary>
public interface IDatabaseApiContainer 
{
    /// <summary>
    /// 获取DatabaseApi
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IDatabaseApi? FindDatabaseApi([NotNull] string key);

    /// <summary>
    /// 添加DatabaseApi
    /// </summary>
    /// <param name="key"></param>
    /// <param name="api"></param>
    void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

    /// <summary>
    /// 获取并添加DatabaseApi
    /// </summary>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
}
