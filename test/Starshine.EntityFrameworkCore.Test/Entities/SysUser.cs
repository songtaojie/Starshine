using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test
{
    public class SysUser:FullAuditedEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string? UserName { get; set; }
    }
}
