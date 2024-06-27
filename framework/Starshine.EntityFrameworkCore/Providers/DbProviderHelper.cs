// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore;
internal class DbProviderHelper
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
    /// MySql 官方包（更新不及时，只支持 8.0.23+ 版本， 所以单独弄一个分类）
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
    /// 构造函数
    /// </summary>
    static DbProviderHelper()
    {
        StarshineDbContextAttributes = new ConcurrentDictionary<Type, StarshineDbContextAttribute?>();
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
    private static readonly ConcurrentDictionary<EfCoreDatabaseProvider, MethodInfo?> DatabaseProviderUseMethods;


    /// <summary>
    /// 获取数据库提供器对应的 useXXX 方法
    /// </summary>
    /// <param name="databaseProvider">数据库提供器</param>
    /// <returns></returns>
    internal static MethodInfo? GetDatabaseProviderUseMethod(EfCoreDatabaseProvider databaseProvider)
    {
        return DatabaseProviderUseMethods.GetOrAdd(databaseProvider, GetOrAddFunction(databaseProvider));

        static MethodInfo? GetOrAddFunction(EfCoreDatabaseProvider databaseProvider)
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
            if (databaseProvider == EfCoreDatabaseProvider.MySql)
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
        var databaseProviderAssembly = GetDatabaseProviderAssembly(EfCoreDatabaseProvider.MySql);

        if (databaseProviderAssembly == null) return null;
        try
        { 
            // 解析mysql版本类型
            var serverVersion = databaseProviderAssembly.GetType("Microsoft.EntityFrameworkCore.ServerVersion");
            if (serverVersion == null) return null;
           
            // 获取静态方法的信息
            var autoDetect = serverVersion.GetMethod("AutoDetect");
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
    private static Assembly? GetDatabaseProviderAssembly(EfCoreDatabaseProvider databaseProvider)
    {
        var providerName = databaseProvider switch
        {
            EfCoreDatabaseProvider.InMemory => InMemoryDatabase,
            EfCoreDatabaseProvider.MySql => MySql,
            EfCoreDatabaseProvider.Sqlite => Sqlite,
            EfCoreDatabaseProvider.SqlServer => SqlServer,
            EfCoreDatabaseProvider.PostgreSQL => PostgreSQL,
            EfCoreDatabaseProvider.MySqlOfficial => MySqlOfficial,
            EfCoreDatabaseProvider.Oracle => Oracle,
            EfCoreDatabaseProvider.Cosmos => Cosmos,
            EfCoreDatabaseProvider.Firebird => Firebird,
            EfCoreDatabaseProvider.Dm => Dm,
            _ => string.Empty
        };
        if (string.IsNullOrEmpty(providerName)) return null;
        
        return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(providerName));
    }

    /// <summary>
    /// 获取对应程序集中的扩展程序类名
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static string? GetDatabaseProviderServiceExtensionTypeName(EfCoreDatabaseProvider databaseProvider)
    {
        var className = databaseProvider switch
        {
            EfCoreDatabaseProvider.SqlServer => "SqlServerDbContextOptionsExtensions",
            EfCoreDatabaseProvider.Sqlite => "SqliteDbContextOptionsBuilderExtensions",
            EfCoreDatabaseProvider.Cosmos => "CosmosDbContextOptionsExtensions",
            EfCoreDatabaseProvider.InMemory => "InMemoryDbContextOptionsExtensions",
            EfCoreDatabaseProvider.MySql => "MySqlDbContextOptionsBuilderExtensions",
            EfCoreDatabaseProvider.MySqlOfficial => "MySQLDbContextOptionsExtensions",
            EfCoreDatabaseProvider.PostgreSQL => "NpgsqlDbContextOptionsBuilderExtensions",
            EfCoreDatabaseProvider.Oracle => "OracleDbContextOptionsExtensions",
            EfCoreDatabaseProvider.Firebird => "FbDbContextOptionsBuilderExtensions",
            EfCoreDatabaseProvider.Dm => "DmDbContextOptionsExtensions",
            _ => null
        };
        return $"Microsoft.EntityFrameworkCore.{className}";
    }

    /// <summary>
    /// 获取对应程序集中的扩展程序类名
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static string? GetDatabaseProviderServiceExtensionUseMethodName(EfCoreDatabaseProvider databaseProvider)
    {
        var useMethodName = databaseProvider switch
        {
            EfCoreDatabaseProvider.SqlServer => $"Use{nameof(EfCoreDatabaseProvider.SqlServer)}",
            EfCoreDatabaseProvider.Sqlite => $"Use{nameof(EfCoreDatabaseProvider.Sqlite)}",
            EfCoreDatabaseProvider.Cosmos => $"Use{nameof(EfCoreDatabaseProvider.Cosmos)}",
            EfCoreDatabaseProvider.InMemory => $"UseInMemoryDatabase",
            EfCoreDatabaseProvider.MySql => $"Use{nameof(EfCoreDatabaseProvider.MySql)}",
            EfCoreDatabaseProvider.MySqlOfficial => $"UseMySQL",
            EfCoreDatabaseProvider.PostgreSQL => $"UseNpgsql",
            EfCoreDatabaseProvider.Oracle => $"Use{nameof(EfCoreDatabaseProvider.Oracle)}",
            EfCoreDatabaseProvider.Firebird => $"Use{nameof(EfCoreDatabaseProvider.Firebird)}",
            EfCoreDatabaseProvider.Dm => $"Use{nameof(EfCoreDatabaseProvider.Dm)}",
            _ => null
        };
        return useMethodName;
    }
}
