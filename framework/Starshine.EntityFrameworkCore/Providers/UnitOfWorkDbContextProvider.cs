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
    protected readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionStringResolver"></param>
    /// <param name="dbContextTypeProvider"></param>
    /// <param name="logger"></param>
    public UnitOfWorkDbContextProvider(
        IConnectionStringResolver connectionStringResolver,
        IDbContextTypeProvider dbContextTypeProvider,
        ILogger<UnitOfWorkDbContextProvider<TDbContext>> logger,
        IUnitOfWork unitOfWork)
    {
        _connectionStringResolver = connectionStringResolver;
        _dbContextTypeProvider = dbContextTypeProvider;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// 获取DbContext
    /// </summary>
    /// <returns></returns>
    public Task<TDbContext> GetDbContextAsync()
    {
       
        var targetDbContextType = _dbContextTypeProvider.GetDbContextType(typeof(TDbContext));
        var connectionString = _connectionStringResolver.ResolveAsync<TDbContext>();

        var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";

        var databaseApi = _unitOfWork.FindDatabaseApi(dbContextKey);

        if (databaseApi == null)
        {
            databaseApi = new EfCoreDatabaseApi(
                await CreateDbContextAsync(unitOfWork, connectionString)
            );

            _unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
    }

    /// <summary>
    /// 创建数据库连接上下文DbContext
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionString)
    {
        var creationContext = new DbContextCreationContext(connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);

            if (dbContext is IAbpEfCoreDbContext abpEfCoreDbContext)
            {
                abpEfCoreDbContext.Initialize(
                    new AbpEfCoreDbContextInitializationContext(
                        unitOfWork
                    )
                );
            }

            return dbContext;
        }
    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var dbTransaction = unitOfWork.Options.IsolationLevel.HasValue
                    ? await dbContext.Database.BeginTransactionAsync(unitOfWork.Options.IsolationLevel.Value, GetCancellationToken())
                    : await dbContext.Database.BeginTransactionAsync(GetCancellationToken());

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction,
                        dbContext,
                        CancellationTokenProvider
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.As<DbContext>().HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction(), GetCancellationToken());
                }
                else
                {
                    try
                    {
                        /* User did not re-use the ExistingConnection and we are starting a new transaction.
                            * EfCoreTransactionApi will check the connection string match and separately
                            * commit/rollback this transaction over the DbContext instance. */
                        if (unitOfWork.Options.IsolationLevel.HasValue)
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                unitOfWork.Options.IsolationLevel.Value,
                                GetCancellationToken()
                            );
                        }
                        else
                        {
                            await dbContext.Database.BeginTransactionAsync(
                                GetCancellationToken()
                            );
                        }
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogWarning(TransactionsNotSupportedWarningMessage);

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
                    await dbContext.Database.BeginTransactionAsync(GetCancellationToken());
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }

}
