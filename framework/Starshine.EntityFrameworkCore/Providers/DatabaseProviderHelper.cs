// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
internal class DatabaseProviderHelper
{

    /// <summary>
    /// SqlServer 提供器程序集
    /// </summary>
    public const string SqlServer = "Microsoft.EntityFrameworkCore.SqlServer";

    /// <summary>
    /// Sqlite 提供器程序集
    /// </summary>
    public const string Sqlite = "Microsoft.EntityFrameworkCore.Sqlite";

    /// <summary>
    /// Cosmos 提供器程序集
    /// </summary>
    public const string Cosmos = "Microsoft.EntityFrameworkCore.Cosmos";

    /// <summary>
    /// 内存数据库 提供器程序集
    /// </summary>
    public const string InMemoryDatabase = "Microsoft.EntityFrameworkCore.InMemory";

    /// <summary>
    /// MySql 提供器程序集
    /// </summary>
    public const string MySql = "Pomelo.EntityFrameworkCore.MySql";

    /// <summary>
    /// MySql 官方包
    /// </summary>
    public const string MySqlOfficial = "MySql.EntityFrameworkCore";

    /// <summary>
    /// PostgreSQL 提供器程序集
    /// </summary>
    public const string PostgreSQL = "Npgsql.EntityFrameworkCore.PostgreSQL";

    /// <summary>
    /// Oracle 提供器程序集
    /// </summary>
    public const string Oracle = "Oracle.EntityFrameworkCore";

    /// <summary>
    /// Firebird 提供器程序集
    /// </summary>
    public const string Firebird = "FirebirdSql.EntityFrameworkCore.Firebird";

    /// <summary>
    /// Dm 提供器程序集
    /// </summary>
    public const string Dm = "Microsoft.EntityFrameworkCore.Dm";

    /// <summary>
    /// 是否开启
    /// </summary>
    public static bool EnabledMiniProfiler {  get; set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    static DatabaseProviderHelper()
    {
        StarshineDbContextAttributes = new();
        DatabaseProviderUseMethods = new();
    }
    /// <summary>
    /// 数据库上下文 [StarshineDbContextAttribute] 特性缓存
    /// </summary>
    private static readonly ConcurrentDictionary<Type, StarshineDbContextAttribute?> StarshineDbContextAttributes;

    /// <summary>
    /// 获取数据库上下文 [StarshineDbContextAttribute] 特性
    /// </summary>
    /// <param name="dbContexType"></param>
    /// <returns></returns>
    internal static StarshineDbContextAttribute? GetStarshineDbContextAttribute(Type dbContexType)
    {
        return StarshineDbContextAttributes.GetOrAdd(dbContexType, GetOrAddFunction);

        // 本地静态函数
        static StarshineDbContextAttribute? GetOrAddFunction(Type dbContextType)
        {
            return dbContextType.GetCustomAttribute<StarshineDbContextAttribute>(true);
        }
    }


    /// <summary>
    /// 数据库提供器 UseXXX 方法缓存集合
    /// </summary>
    private static readonly ConcurrentDictionary<EFCoreDatabaseProvider, MethodInfo?> DatabaseProviderUseMethods;


    /// <summary>
    /// 获取数据库提供器对应的 useXXX 方法
    /// </summary>
    /// <param name="databaseProvider">数据库提供器</param>
    /// <returns></returns>
    internal static MethodInfo? GetDatabaseProviderUseMethod(EFCoreDatabaseProvider databaseProvider)
    {
        return DatabaseProviderUseMethods.GetOrAdd(databaseProvider, GetOrAddFunction(databaseProvider));

        static MethodInfo? GetOrAddFunction(EFCoreDatabaseProvider databaseProvider)
        {
            // 加载对应的数据库提供器程序集
            var databaseProviderAssembly = GetDatabaseProviderAssembly(databaseProvider);

            if (databaseProviderAssembly == null) return null;

            // 数据库提供器服务拓展类型名
            var databaseProviderServiceExtensionTypeName = GetDatabaseProviderServiceExtensionTypeName(databaseProvider);
            if(string.IsNullOrWhiteSpace(databaseProviderServiceExtensionTypeName)) return null;

            // 加载拓展类型
            var databaseProviderServiceExtensionType = databaseProviderAssembly.GetType(databaseProviderServiceExtensionTypeName);
            if(databaseProviderServiceExtensionType == null) return null;

            // useXXX方法名
            var useMethodName = GetDatabaseProviderServiceExtensionUseMethodName(databaseProvider);

            // 获取UseXXX方法
            MethodInfo? useMethod;

            // 处理最新 MySql 第三方包兼容问题
            if (databaseProvider == EFCoreDatabaseProvider.MySql)
            {
                useMethod = databaseProviderServiceExtensionType
                   .GetMethods(BindingFlags.Public | BindingFlags.Static)
                   .FirstOrDefault(u => u.Name == useMethodName && !u.IsGenericMethod && u.GetParameters().Length == 4 && u.GetParameters()[1].ParameterType == typeof(string));
            }
            else
            {
                useMethod = databaseProviderServiceExtensionType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(u => u.Name == useMethodName && !u.IsGenericMethod && u.GetParameters().Length == 3 && u.GetParameters()[1].ParameterType == typeof(string));
            }
            return useMethod;
        }
    }

    /// <summary>
    /// 连接字符串
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    internal static object? GetMySqlVersion(string connectionString)
    {
        // 加载对应的数据库提供器程序集
        var databaseProviderAssembly = GetDatabaseProviderAssembly(EFCoreDatabaseProvider.MySql);

        if (databaseProviderAssembly == null) return null;
        try
        { 
            // 解析mysql版本类型
            var serverVersion = databaseProviderAssembly.GetType("Microsoft.EntityFrameworkCore.ServerVersion");
            if (serverVersion == null) return null;

            // 获取静态方法的信息
            var autoDetect = serverVersion.GetMethod("AutoDetect", BindingFlags.Static | BindingFlags.Public,new Type[] {typeof(string) });
            if(autoDetect == null) return null;
            // 调用静态方法
            return autoDetect.Invoke(null, new object[] { connectionString });
        }
        catch
        { 
            return null; 
        }
    }

    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static Assembly? GetDatabaseProviderAssembly(EFCoreDatabaseProvider databaseProvider)
    {
        var providerName = databaseProvider switch
        {
            EFCoreDatabaseProvider.InMemory => InMemoryDatabase,
            EFCoreDatabaseProvider.MySql => MySql,
            EFCoreDatabaseProvider.Sqlite => Sqlite,
            EFCoreDatabaseProvider.SqlServer => SqlServer,
            EFCoreDatabaseProvider.PostgreSQL => PostgreSQL,
            EFCoreDatabaseProvider.MySqlOfficial => MySqlOfficial,
            EFCoreDatabaseProvider.Oracle => Oracle,
            EFCoreDatabaseProvider.Cosmos => Cosmos,
            EFCoreDatabaseProvider.Firebird => Firebird,
            EFCoreDatabaseProvider.Dm => Dm,
            _ => throw new NotSupportedException($"The database provider {databaseProvider} does not support.")
        };
        if (string.IsNullOrEmpty(providerName)) return null;
        
        return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(providerName));
    }

    /// <summary>
    /// 获取对应程序集中的扩展程序类名
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static string? GetDatabaseProviderServiceExtensionTypeName(EFCoreDatabaseProvider databaseProvider)
    {
        var className = databaseProvider switch
        {
            EFCoreDatabaseProvider.SqlServer => "SqlServerDbContextOptionsExtensions",
            EFCoreDatabaseProvider.Sqlite => "SqliteDbContextOptionsBuilderExtensions",
            EFCoreDatabaseProvider.Cosmos => "CosmosDbContextOptionsExtensions",
            EFCoreDatabaseProvider.InMemory => "InMemoryDbContextOptionsExtensions",
            EFCoreDatabaseProvider.MySql => "MySqlDbContextOptionsBuilderExtensions",
            EFCoreDatabaseProvider.MySqlOfficial => "MySQLDbContextOptionsExtensions",
            EFCoreDatabaseProvider.PostgreSQL => "NpgsqlDbContextOptionsBuilderExtensions",
            EFCoreDatabaseProvider.Oracle => "OracleDbContextOptionsExtensions",
            EFCoreDatabaseProvider.Firebird => "FbDbContextOptionsBuilderExtensions",
            EFCoreDatabaseProvider.Dm => "DmDbContextOptionsExtensions",
            _ => throw new NotSupportedException($"The database provider {databaseProvider} does not support.")
        };
        return $"Microsoft.EntityFrameworkCore.{className}";
    }

    /// <summary>
    /// 获取对应程序集中的扩展程序类名
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static string? GetDatabaseProviderServiceExtensionUseMethodName(EFCoreDatabaseProvider databaseProvider)
    {
        var useMethodName = databaseProvider switch
        {
            EFCoreDatabaseProvider.SqlServer => $"Use{nameof(EFCoreDatabaseProvider.SqlServer)}",
            EFCoreDatabaseProvider.Sqlite => $"Use{nameof(EFCoreDatabaseProvider.Sqlite)}",
            EFCoreDatabaseProvider.Cosmos => $"Use{nameof(EFCoreDatabaseProvider.Cosmos)}",
            EFCoreDatabaseProvider.InMemory => $"UseInMemoryDatabase",
            EFCoreDatabaseProvider.MySql => $"Use{nameof(EFCoreDatabaseProvider.MySql)}",
            EFCoreDatabaseProvider.MySqlOfficial => $"UseMySQL",
            EFCoreDatabaseProvider.PostgreSQL => $"UseNpgsql",
            EFCoreDatabaseProvider.Oracle => $"Use{nameof(EFCoreDatabaseProvider.Oracle)}",
            EFCoreDatabaseProvider.Firebird => $"Use{nameof(EFCoreDatabaseProvider.Firebird)}",
            EFCoreDatabaseProvider.Dm => $"Use{nameof(EFCoreDatabaseProvider.Dm)}",
            _ => throw new NotSupportedException($"The database provider {databaseProvider} does not support.")
        };
        return useMethodName;
    }
    /// <summary>
    /// 检查是否支持存储过程
    /// </summary>
    /// <param name="providerName">数据库提供器名词</param>
    /// <param name="commandType">命令类型</param>
    internal static void CheckIsSupportedStoredProcedure(string? providerName, CommandType commandType)
    {
        if (commandType == CommandType.StoredProcedure && new string[] {Sqlite,InMemoryDatabase }.Contains(providerName))
        {
            throw new NotSupportedException("The database provider does not support stored procedure operations.");
        }
    }

    /// <summary>
    /// 判断是否是特定数据库
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="dbAssemblyName"></param>
    /// <returns>bool</returns>
    public static bool IsDatabaseFor(string? providerName, string dbAssemblyName)
    {
        return dbAssemblyName.Equals(providerName, StringComparison.Ordinal);
    }
}
