using Hx.DatabaseAccessor.Internal;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseAccessorApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加数据库访问中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDatabaseAccessor(this IApplicationBuilder app)
        {
            Penetrates.ServiceProvider = app.ApplicationServices;

            if (Penetrates.DbSettings.EnabledMiniProfiler == true)
            {
                app.UseMiniProfiler();
            }
            return app;
        }
    }
}
