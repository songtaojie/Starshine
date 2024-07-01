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
/// UnitOfWork管理
/// </summary>
public interface IUnitOfWorkManager
{
    /// <summary>
    /// 当前运行的UnitOfWork
    /// </summary>
    IUnitOfWork? Current { get; }

    /// <summary>
    /// 开始工作单元
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    IUnitOfWork Begin(UnitOfWorkOptions options);
}
