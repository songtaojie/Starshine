// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 工作单元依赖接口
/// </summary>
public interface IUnitOfWork: IDatabaseApiContainer,ITransactionApiContainer,IDisposable
{
    /// <summary>
    /// 正在使用的Starshine.EntityFrameworkCore.IUnitOfWork的唯一标识符。
    /// </summary>
    Guid InstanceId { get; }

    /// <summary>
    /// 是否已经释放
    /// </summary>
    bool IsDisposed { get; }

    /// <summary>
    /// 返回已经完成
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// 完成
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    ///// <summary>
    ///// 开启工作单元处理
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="options"></param>
    //void BeginTransaction(FilterContext context, UnitOfWorkOptions options);

    ///// <summary>
    ///// 提交工作单元处理
    ///// </summary>
    ///// <param name="resultContext"></param>
    ///// <param name="options"></param>
    //void CommitTransaction(FilterContext resultContext, UnitOfWorkOptions options);

    ///// <summary>
    ///// 回滚工作单元处理
    ///// </summary>
    ///// <param name="resultContext"></param>
    ///// <param name="options"></param>
    //void RollbackTransaction(FilterContext resultContext, UnitOfWorkOptions options);

    ///// <summary>
    ///// 执行完毕（无论成功失败）
    ///// </summary>
    ///// <param name="context"></param>
    ///// <param name="resultContext"></param>
    //void OnCompleted(FilterContext context, FilterContext resultContext);
}
