// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using Starshine.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 工作单元
/// </summary>
public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
{
    private readonly AsyncLocal<IUnitOfWork?> _currentUow;
    /// <summary>
    /// 
    /// </summary>
    public IUnitOfWork? Current => GetCurrentByChecking();

    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public UnitOfWorkManager(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _currentUow = new AsyncLocal<IUnitOfWork?>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IUnitOfWork Begin(UnitOfWorkOptions options)
    {
        if(options == null)
            throw new ArgumentNullException(nameof(options));

        var unitOfWork = CreateNewUnitOfWork();
        unitOfWork.Initialize(options);

        return unitOfWork;
    }

    private IUnitOfWork CreateNewUnitOfWork()
    {
        var scope = _serviceScopeFactory.CreateScope();
        try
        {
            var outerUow = _currentUow.Value;

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            unitOfWork.SetOuter(outerUow);

            SetUnitOfWork(unitOfWork);

            unitOfWork.Disposed += (sender, args) =>
            {
                SetUnitOfWork(outerUow);
                scope.Dispose();
            };

            return unitOfWork;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }

    
    private void SetUnitOfWork(IUnitOfWork? unitOfWork)
    {
        _currentUow.Value = unitOfWork;
    }

    private IUnitOfWork? GetCurrentByChecking()
    {
        var uow = _currentUow.Value;

        while (uow != null && (uow.IsDisposed || uow.IsCompleted))
        {
            uow = uow.Outer;
        }

        return uow;
    }
}
