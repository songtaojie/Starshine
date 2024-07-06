using Starshine.DependencyInjection;
using Starshine.EntityFrameworkCore.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test.Services
{
    public class ShopService : EFCoreRepository<ShopDbContext, Shop>, IShopService, ITransientDependency
    {
        public ShopService(IServiceProvider scoped) : base(scoped)
        {
        }
    }
}
