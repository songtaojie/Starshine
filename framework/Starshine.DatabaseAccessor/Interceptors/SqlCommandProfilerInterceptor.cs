using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Starshine.DatabaseAccessor
{
    /// <summary>
    /// 数据库执行命令拦截
    /// </summary>
    internal sealed class SqlCommandProfilerInterceptor : DbCommandInterceptor
    {
    }
}