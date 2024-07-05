using Starshine.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test
{
    public class SysUserService : EFCoreRepository<UserDbContext, SysUser>, ISysUserService,ITransientDependency
    {
        public SysUserService(IServiceProvider scoped) : base(scoped)
        {
        }
    }
}
