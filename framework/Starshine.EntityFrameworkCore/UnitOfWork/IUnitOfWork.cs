// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    /// 
    /// </summary>
    IServiceProvider ServiceProvider { get; }
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
    /// 
    /// </summary>
    IUnitOfWork? Outer { get; }

    /// <summary>
    /// 
    /// </summary>
    event EventHandler<UnitOfWorkEventArgs> Disposed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="outer"></param>
    void SetOuter(IUnitOfWork? outer);

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="options"></param>
    void Initialize([NotNull] UnitOfWorkOptions options);

    /// <summary>
    /// 
    /// </summary>
    UnitOfWorkOptions Options { get; }

    /// <summary>
    /// 完成
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
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
