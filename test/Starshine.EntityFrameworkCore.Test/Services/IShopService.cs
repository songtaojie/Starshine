using Starshine.EntityFrameworkCore.Test.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test.Services
{
    public interface IShopService : IEFCoreRepository<Shop>
    {
    }
}
