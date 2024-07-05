using Microsoft.EntityFrameworkCore;
using Starshine.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test
{
    public class UserDbContextModelBuilderFilter : IModelBuilderFilter<UserDbContext>,ITransientDependency
    {
        public void OnModelCreating(ModelBuilder modelBuilder, DbContext dbContext)
        {
        }

        public void OnOnModelCreated(ModelBuilder modelBuilder, DbContext dbContext)
        {
        }
    }
}
