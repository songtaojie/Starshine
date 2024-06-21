using System;

namespace Starshine.EntityFrameworkCore
{
    /// <summary>
    /// 构建 Sql 字符串执行部件
    /// </summary>
    public sealed partial class SqlStringExecutePart
    {
        /// <summary>
        /// Sql 字符串
        /// </summary>
        public string SqlString { get; private set; }

        /// <summary>
        /// 数据库上下文定位器
        /// </summary>
        public Type DbContextLocator { get; private set; } = typeof(DefaultDbContextTypeProvider);

        /// <summary>
        /// 设置服务提供器
        /// </summary>
        public IServiceProvider ContextScoped { get; private set; }
    }
}