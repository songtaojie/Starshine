using System;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StarshineDbContextAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public StarshineDbContextAttribute(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="provider"></param>
        public StarshineDbContextAttribute(string connectionString, EfCoreDatabaseProvider provider)
        {
            ConnectionString = connectionString;
            Provider = provider;
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库提供器名称
        /// </summary>
        public EfCoreDatabaseProvider? Provider { get; set; }

        /// <summary>
        /// 数据库上下文模式
        /// </summary>
        public DbContextMode Mode { get; set; } = DbContextMode.Cached;

        /// <summary>
        /// 表统一前缀
        /// </summary>
        public string? TablePrefix { get; set; }

        /// <summary>
        /// 表统一后缀
        /// </summary>
        public string? TableSuffix { get; set; }
    }
}