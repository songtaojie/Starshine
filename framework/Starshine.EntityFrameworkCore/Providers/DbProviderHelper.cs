// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

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
    private static readonly ConcurrentDictionary<string, MethodInfo> DatabaseProviderUseMethods;


    /// <summary>
    /// 获取数据库提供器对应的 useXXX 方法
    /// </summary>
    /// <param name="databaseProvider">数据库提供器</param>
    /// <param name="version"></param>
    /// <returns></returns>
    private static (MethodInfo UseMethod, object MySqlVersion) GetDatabaseProviderUseMethod(DatabaseProvider databaseProvider, string version)
    {
        return DatabaseProviderUseMethods.GetOrAdd(providerName, GetOrAddFunction(databaseProvider, version));

        static (MethodInfo, object) GetOrAddFunction(DatabaseProvider databaseProvider, string version)
        {
            // 处理最新 MySql 包兼容问题
            // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/commit/83c699f5b747253dc1b6fa9c470f469467d77686
            object mySqlVersionInstance = default;

            // 加载对应的数据库提供器程序集
            var databaseProviderAssembly = GetDatabaseProviderAssembly(databaseProvider);

            // 数据库提供器服务拓展类型名
            var databaseProviderServiceExtensionTypeName = GetDatabaseProviderServiceExtensionTypeName(databaseProvider);

            // 加载拓展类型
            var databaseProviderServiceExtensionType = databaseProviderAssembly.GetType($"Microsoft.EntityFrameworkCore.{databaseProviderServiceExtensionTypeName}");

            // useXXX方法名
            var useMethodName = providerName switch
            {
                DbProvider.SqlServer => $"Use{nameof(DbProvider.SqlServer)}",
                DbProvider.Sqlite => $"Use{nameof(DbProvider.Sqlite)}",
                DbProvider.Cosmos => $"Use{nameof(DbProvider.Cosmos)}",
                DbProvider.InMemoryDatabase => $"Use{nameof(DbProvider.InMemoryDatabase)}",
                DbProvider.MySql => $"Use{nameof(DbProvider.MySql)}",
                DbProvider.MySqlOfficial => $"UseMySQL",
                DbProvider.Npgsql => $"Use{nameof(DbProvider.Npgsql)}",
                DbProvider.Oracle => $"Use{nameof(DbProvider.Oracle)}",
                DbProvider.Firebird => $"Use{nameof(DbProvider.Firebird)}",
                DbProvider.Dm => $"Use{nameof(DbProvider.Dm)}",
                _ => null
            };

            // 获取UseXXX方法
            MethodInfo useMethod;

            // 处理最新 MySql 第三方包兼容问题
            // https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/commit/83c699f5b747253dc1b6fa9c470f469467d77686
            if (DbProvider.IsDatabaseFor(providerName, DbProvider.MySql))
            {
                useMethod = databaseProviderServiceExtensionType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(u => u.Name == useMethodName && !u.IsGenericMethod && u.GetParameters().Length == 4 && u.GetParameters()[1].ParameterType == typeof(string));

                // 解析mysql版本类型
                var mysqlVersionType = databaseProviderAssembly.GetType("Microsoft.EntityFrameworkCore.MySqlServerVersion");
                mySqlVersionInstance = Activator.CreateInstance(mysqlVersionType, new object[] { new Version(version ?? "8.0.22") });
            }
            else
            {
                useMethod = databaseProviderServiceExtensionType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(u => u.Name == useMethodName && !u.IsGenericMethod && u.GetParameters().Length == 3 && u.GetParameters()[1].ParameterType == typeof(string));
            }

            return (useMethod, mySqlVersionInstance);
        }
    }


    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="databaseProvider"></param>
    /// <returns></returns>
    private static Assembly? GetDatabaseProviderAssembly(DatabaseProvider databaseProvider)
    {
        var providerName = databaseProvider switch
        {
            DatabaseProvider.InMemory => InMemoryDatabase,
            DatabaseProvider.MySql => MySql,
            DatabaseProvider.Sqlite => Sqlite,
            DatabaseProvider.SqlServer => SqlServer,
            DatabaseProvider.PostgreSQL => PostgreSQL,
            DatabaseProvider.MySqlOfficial => MySqlOfficial,
            DatabaseProvider.Oracle => Oracle,
            DatabaseProvider.Cosmos => Cosmos,
            DatabaseProvider.Firebird => Firebird,
            DatabaseProvider.Dm => Dm,
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
    private static string? GetDatabaseProviderServiceExtensionTypeName(DatabaseProvider databaseProvider)
    {
        var className = databaseProvider switch
        {
            DatabaseProvider.SqlServer => "SqlServerDbContextOptionsExtensions",
            DatabaseProvider.Sqlite => "SqliteDbContextOptionsBuilderExtensions",
            DatabaseProvider.Cosmos => "CosmosDbContextOptionsExtensions",
            DatabaseProvider.InMemory => "InMemoryDbContextOptionsExtensions",
            DatabaseProvider.MySql => "MySqlDbContextOptionsBuilderExtensions",
            DatabaseProvider.MySqlOfficial => "MySQLDbContextOptionsExtensions",
            DatabaseProvider.PostgreSQL => "NpgsqlDbContextOptionsBuilderExtensions",
            DatabaseProvider.Oracle => "OracleDbContextOptionsExtensions",
            DatabaseProvider.Firebird => "FbDbContextOptionsBuilderExtensions",
            DatabaseProvider.Dm => "DmDbContextOptionsExtensions",
            _ => null
        };
        return $"Microsoft.EntityFrameworkCore.{className}";
    }
}
