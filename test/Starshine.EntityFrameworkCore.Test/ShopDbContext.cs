using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test
{
    [StarshineDbContext("Shop",EFCoreDatabaseProvider.MySql)]
    public class ShopDbContext : StarshineDbContext<ShopDbContext>
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }
    }
}
