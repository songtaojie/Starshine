// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;

namespace Starshine.EntityFrameworkCore;
/// <summary>
/// 默认实现
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    private const string TransactionsNotSupportedWarningMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";

    private readonly ILogger _logger;

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    protected readonly IConnectionStringResolver _connectionStringResolver;

    /// <summary>
    /// DbContext提供者
    /// </summary>
    protected readonly IDbContextTypeProvider _dbContextTypeProvider;

    /// <summary>
    /// DbContext提供者
    /// </summary>
    protected readonly IUnitOfWorkManager _unitOfWorkManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionStringResolver"></param>
    /// <param name="dbContextTypeProvider"></param>
    /// <param name="logger"></param>
    /// <param name="unitOfWorkManager"></param>
    public UnitOfWorkDbContextProvider(
        IConnectionStringResolver connectionStringResolver,
        IDbContextTypeProvider dbContextTypeProvider,
        ILogger<UnitOfWorkDbContextProvider<TDbContext>> logger,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _connectionStringResolver = connectionStringResolver;
        _dbContextTypeProvider = dbContextTypeProvider;
        _logger = logger;
        _unitOfWorkManager = unitOfWorkManager;
    }

    /// <summary>
    /// 获取DbContext
    /// </summary>
    /// <returns></returns>
    public async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new Exception("A DbContext can only be created inside a unit of work!");
        }
        var targetDbContextType = _dbContextTypeProvider.GetDbContextType(typeof(TDbContext));
        var connectionString = await _connectionStringResolver.ResolveAsync<TDbContext>();

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            databaseApi = new EfCoreDatabaseApi(await CreateDbContextAsync(unitOfWork, connectionString));
            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }
        return (TDbContext)((EfCoreDatabaseApi)databaseApi).StarterDbContext;
    }

    /// <summary>
    /// 创建数据库连接上下文DbContext
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="connectionString"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string? connectionString, CancellationToken cancellationToken = default)
    {
        var creationContext = new DbContextCreationContext(connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);

            if (dbContext is StarshineDbContext<TDbContext> starshineDbContext)
            {
                starshineDbContext.Initialize(unitOfWork.Options);
            }

            return dbContext;
        }
    }

    /// <summary>
    /// 创建DbContext
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork, cancellationToken)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    /// <summary>
    /// 创建带事务的DbContext
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, cancellationToken)
                    : await dbContext.Database.BeginTransactionAsync(cancellationToken);

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                _logger.LogWarning(TransactionsNotSupportedWarningMessage);

                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.Database.IsRelational())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), cancellationToken);
                }
                else
                {
                    try
                    {
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Options.IsolationLevel.Value,
                               cancellationToken
                            );
                        }
                        else
                        {
                            await dbContext.Database.BeginTransactionAsync(cancellationToken);
                        }
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        _logger.LogWarning(TransactionsNotSupportedWarningMessage);
                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    /* No need to store the returning IDbContextTransaction for non-relational databases
                        * since EfCoreTransactionApi will handle the commit/rollback over the DbContext instance.
                          */
                    await dbContext.Database.BeginTransactionAsync(cancellationToken);
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    _logger.LogWarning(TransactionsNotSupportedWarningMessage);
                    return dbContext;
                }
            }
            activeTransaction.AttendedDbContexts.Add(dbContext);
            return dbContext;
        }
    }

}
