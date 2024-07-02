// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Options;
using Starshine.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// EFCore 工作单元实现
/// </summary>
public class EFCoreUnitOfWork : IUnitOfWork, ITransientDependency
{
    /// <summary>
    /// ServiceProvider
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// 线程安全的DatabaseApi集合
    /// </summary>
    private readonly ConcurrentDictionary<string, IDatabaseApi> _databaseApis;

    /// <summary>
    /// 线程安全的TransactionApi集合
    /// </summary>
    private readonly ConcurrentDictionary<string, ITransactionApi> _transactionApis;

    /// <summary>
    /// 当前请求id
    /// </summary>
    public Guid InstanceId { get; } = Guid.NewGuid();

    /// <summary>
    /// 资源是否释放
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// 是否已经完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public UnitOfWorkOptions Options { get; private set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<UnitOfWorkEventArgs> Disposed = default!;

    /// <summary>
    /// 
    /// </summary>
    public IUnitOfWork? Outer { get; private set; }

    /// <summary>
    /// 是否回滚
    /// </summary>
    private bool _isRolledback;

    /// <summary>
    /// 是否正在完成
    /// </summary>
    private bool _isCompleting;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public EFCoreUnitOfWork(
       IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        _databaseApis = new();
        _transactionApis = new();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="options"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Initialize([NotNull] UnitOfWorkOptions options)
    {
        if(options == null)
            throw new ArgumentNullException(nameof(options));

        if (Options != null)
        {
            throw new InvalidOperationException("This unit of work has already been initialized.");
        }
        Options = options.Clone();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="outer"></param>
    public virtual void SetOuter(IUnitOfWork? outer)
    {
        Outer = outer;
    }

    /// <summary>
    /// 获取所有活动的DatabaseApi
    /// </summary>
    /// <returns></returns>
    public virtual IReadOnlyList<IDatabaseApi> GetAllActiveDatabaseApis()
    {
        return _databaseApis.Values.ToImmutableList();
    }

    /// <summary>
    /// 获取所有活动的TransactionApi
    /// </summary>
    /// <returns></returns>
    public virtual IReadOnlyList<ITransactionApi> GetAllActiveTransactionApis()
    {
        return _transactionApis.Values.ToImmutableList();
    }

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback) return;
        _isRolledback = true;
        await RollbackAllAsync(cancellationToken);
    }

    /// <summary>
    /// 根据键获取DatabaseApi
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IDatabaseApi? FindDatabaseApi([NotNull] string key)
    {
       return _databaseApis.TryGetValue(key, out IDatabaseApi? api)? api:default;
    }

    /// <summary>
    /// 添加DatabaseApi
    /// </summary>
    /// <param name="key"></param>
    /// <param name="api"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public virtual void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"key can not be null, empty or white space!", nameof(key));
        if(api == null)
            throw new ArgumentNullException(nameof(api));
        if (_databaseApis.ContainsKey(key))
        {
            throw new Exception("This unit of work already contains a database API for the given key.");
        }
        _databaseApis.TryAdd(key, api);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"key can not be null, empty or white space!", nameof(key));
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        return _databaseApis.GetOrAdd(key, key=> factory.Invoke());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public virtual ITransactionApi? FindTransactionApi([NotNull] string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"key can not be null, empty or white space!", nameof(key));
        return _transactionApis.TryGetValue(key, out ITransactionApi? api) ? api : default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="api"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public virtual void AddTransactionApi([NotNull] string key, [NotNull] ITransactionApi api)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"key can not be null, empty or white space!", nameof(key));
        if (api == null)
            throw new ArgumentNullException(nameof(api));
        if (_transactionApis.ContainsKey(key))
        {
            throw new Exception("This unit of work already contains a transaction API for the given key.");
        }

        _transactionApis.TryAdd(key, api);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual ITransactionApi GetOrAddTransactionApi([NotNull] string key, [NotNull] Func<ITransactionApi> factory)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"key can not be null, empty or white space!", nameof(key));
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        return _transactionApis.GetOrAdd(key, key => factory.Invoke());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback)return;

        foreach (var databaseApi in GetAllActiveDatabaseApis())
        {
            if (databaseApi is IDatabaseApi supportsSavingChangesDatabaseApi)
            {
                await supportsSavingChangesDatabaseApi.SaveChangesAsync(cancellationToken);
            }
        }
    }

    /// <summary>
    /// 回滚
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task RollbackAllAsync(CancellationToken cancellationToken)
    {
        
        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            if (transactionApi is ITransactionApi supportsRollbackTransactionApi)
            {
                try
                {
                    await supportsRollbackTransactionApi.RollbackAsync(cancellationToken);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task CommitTransactionsAsync(CancellationToken cancellationToken)
    {
        foreach (var transaction in GetAllActiveTransactionApis())
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 完成
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (_isRolledback) await Task.CompletedTask;
        PreventMultipleComplete();
        try
        {
            _isCompleting = true;
            await SaveChangesAsync(cancellationToken);

            await CommitTransactionsAsync(cancellationToken);
            IsCompleted = true;
        }
        catch 
        {
            throw;
        }
    }

    /// <summary>
    /// 阻止多次完成
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void PreventMultipleComplete()
    {
        if (IsCompleted || _isCompleting)
        {
            throw new Exception("Completion has already been requested for this unit of work.");
        }
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public virtual void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        IsDisposed = true;
        DisposeTransactions();
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }

    private void DisposeTransactions()
    {
        foreach (var transactionApi in GetAllActiveTransactionApis())
        {
            try
            {
                transactionApi.Dispose();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 重写
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[UnitOfWork {InstanceId}]";
    }

    
}
