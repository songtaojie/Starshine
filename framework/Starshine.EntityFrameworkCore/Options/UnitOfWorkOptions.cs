// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// UnitOfWork配置选项
/// </summary>
public class UnitOfWorkOptions
{
    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsTransactional { get; set; }

    /// <summary>
    /// 隔离级别
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public int? Timeout { get; set; }

    /// <summary>
    /// 克隆
    /// </summary>
    /// <returns></returns>
    public UnitOfWorkOptions Clone()
    {
        return new UnitOfWorkOptions
        {
            IsTransactional = IsTransactional,
            IsolationLevel = IsolationLevel,
            Timeout = Timeout
        };
    }
}
