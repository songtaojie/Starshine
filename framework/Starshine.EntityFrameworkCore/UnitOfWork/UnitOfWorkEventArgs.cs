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
/// 
/// </summary>
public class UnitOfWorkEventArgs : EventArgs
{
    /// <summary>
    /// Reference to the unit of work related to this event.
    /// </summary>
    public IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UnitOfWorkEventArgs([NotNull] IUnitOfWork unitOfWork)
    {
        if(unitOfWork == null)
            throw new ArgumentNullException(nameof(unitOfWork));
        UnitOfWork = unitOfWork;
    }
}
