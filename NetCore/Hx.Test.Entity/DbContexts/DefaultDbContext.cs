//using Hx.Sdk.DatabaseAccessor;
//using Microsoft.EntityFrameworkCore;

//namespace Hx.Test.Entity
//{
//    /// <summary>
//    /// dbcontext
//    /// 生成migration 在启动项目Hx.Sdk.WebApi中执行命令 dotnet ef   -p ../Hx.Test.Entity  migrations add InitTable  -c DefaultDbContext
//    /// </summary>
//    [AppDbContext("MySqlConnectionString", DbProvider.MySqlOfficial)]
//    public class DefaultDbContext : AppDbContext<DefaultDbContext>
//    {
//        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
//        {
//        }
//    }
//}