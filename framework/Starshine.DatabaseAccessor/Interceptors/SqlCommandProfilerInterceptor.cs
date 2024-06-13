using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hx.DatabaseAccessor
{
    /// <summary>
    /// 数据库执行命令拦截
    /// </summary>
    internal sealed class SqlCommandProfilerInterceptor : DbCommandInterceptor
    {
    }
}