using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test.EntityTypeConfiguration
{
    public class SysUserEntityTypeConfiguration:FullAuditedEntityBaseTypeConfiguration<SysUser>
    {
        public override void Configure(EntityTypeBuilder<SysUser> builder)
        {
            base.Configure(builder);
        }
    }
}
