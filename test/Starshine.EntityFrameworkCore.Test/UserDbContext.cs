using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test
{
    [StarshineDbContext("Default",EFCoreDatabaseProvider.MySql)]
    public class UserDbContext : StarshineDbContext<UserDbContext>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<SysUser> SysUsers { get; set; }
    }
}
