using Hx.DatabaseAccessor.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库提供器选项
    /// </summary>
    public static class DbProvider
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
        public const string Npgsql = "Npgsql.EntityFrameworkCore.PostgreSQL";

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
        /// 不支持存储过程的数据库
        /// </summary>
        internal static readonly string[] NotSupportStoredProcedureDatabases;

        /// <summary>
        /// 不支持函数的数据库
        /// </summary>
        internal static readonly string[] NotSupportFunctionDatabases;

        /// <summary>
        /// 不支持表值函数的数据库
        /// </summary>
        internal static readonly string[] NotSupportTableFunctionDatabases;

        /// <summary>
        /// 不支持环境事务的数据库
        /// </summary>
        internal static readonly string[] NotSupportTransactionScopeDatabase;

        /// <summary>
        /// 构造函数
        /// </summary>
        static DbProvider()
        {
            NotSupportStoredProcedureDatabases = new[]
            {
                Sqlite,
                InMemoryDatabase
            };

            NotSupportFunctionDatabases = new[]
            {
                Sqlite,
                InMemoryDatabase,
                Firebird,
                Dm,
            };

            NotSupportTableFunctionDatabases = new[]
            {
                Sqlite,
                InMemoryDatabase,
                MySql,
                MySqlOfficial,
                Firebird,
                Dm
            };

            NotSupportTransactionScopeDatabase = new[]
            {
                Sqlite,
                InMemoryDatabase
            };

            DbContextAppDbContextAttributes = new ConcurrentDictionary<Type, AppDbContextAttribute>();
        }

        /// <summary>
        /// 判断是否是特定数据库
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="dbAssemblyName"></param>
        /// <returns>bool</returns>
        public static bool IsDatabaseFor(string providerName, string dbAssemblyName)
        {
            return providerName.Equals(dbAssemblyName, StringComparison.Ordinal);
        }

        /// <summary>
        /// 获取数据库上下文连接字符串
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetConnectionString<TDbContext>(string connectionString = default)
            where TDbContext : DbContext
        {
            if (!string.IsNullOrWhiteSpace(connectionString)) return connectionString;

            // 如果没有配置数据库连接字符串，那么查找特性
            var dbContextAttribute = GetAppDbContextAttribute(typeof(TDbContext));
            if (dbContextAttribute == null) return default;

            // 获取特性连接字符串
            var connStr = dbContextAttribute.ConnectionString;

            if (string.IsNullOrWhiteSpace(connStr)) return default;
            // 如果包含 = 符号，那么认为是连接字符串
            if (connStr.Contains("=")) return connStr;
            else
            {
                var configuration = Penetrates.GetService<IConfiguration>();
                
                // 如果包含 : 符号，那么认为是一个 Key 路径
                if (connStr.Contains(":")) return configuration[connStr];
                else
                {
                    // 首先查找 DbConnectionString 键，如果没有找到，则当成 Key 去查找
                    var connStrValue = configuration.GetConnectionString(connStr);
                    return !string.IsNullOrWhiteSpace(connStrValue) ? connStrValue : configuration[connStr];
                }
            }
        }

        /// <summary>
        /// 获取默认拦截器
        /// </summary>
        public static List<IInterceptor> GetDefaultInterceptors()
        {
            return new List<IInterceptor>
            {
               
                new SqlCommandProfilerInterceptor(),
            };
        }

        /// <summary>
        /// 数据库上下文 [AppDbContext] 特性缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, AppDbContextAttribute> DbContextAppDbContextAttributes;

        /// <summary>
        /// 获取数据库上下文 [AppDbContext] 特性
        /// </summary>
        /// <param name="dbContexType"></param>
        /// <returns></returns>
        internal static AppDbContextAttribute GetAppDbContextAttribute(Type dbContexType)
        {
            return DbContextAppDbContextAttributes.GetOrAdd(dbContexType, Function);

            // 本地静态函数
            static AppDbContextAttribute Function(Type dbContextType)
            {
                if (!dbContextType.IsDefined(typeof(AppDbContextAttribute), true)) return default;

                var appDbContextAttribute = dbContextType.GetCustomAttribute<AppDbContextAttribute>(true);

                return appDbContextAttribute;
            }
        }

        /// <summary>
        /// 不支持操作类型
        /// </summary>
        private const string NotSupportException = "The database provider does not support {0} operations.";

        /// <summary>
        /// 检查是否支持存储过程
        /// </summary>
        /// <param name="providerName">数据库提供器名词</param>
        /// <param name="commandType">命令类型</param>
        internal static void CheckStoredProcedureSupported(string providerName, CommandType commandType)
        {
            if (commandType == CommandType.StoredProcedure && NotSupportStoredProcedureDatabases.Contains(providerName))
            {
                throw new NotSupportedException(string.Format(NotSupportException, "stored procedure"));
            }
        }
      
    }
}