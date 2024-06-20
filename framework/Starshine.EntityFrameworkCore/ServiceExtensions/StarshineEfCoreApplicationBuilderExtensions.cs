using Starshine.EntityFrameworkCore.Internal;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class StarshineEfCoreApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加数据库访问监听中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDatabaseMiniProfiler(this IApplicationBuilder app)
        {
            DbContextHelper.ServiceProvider = app.ApplicationServices;

            if (DbContextHelper.DbSettings.EnabledMiniProfiler == true)
            {
                app.UseMiniProfiler();
            }
            return app;
        }
    }
}
