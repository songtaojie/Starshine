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
            app.UseMiniProfiler();
            return app;
        }
    }
}
