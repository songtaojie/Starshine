using Starshine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.EntityFrameworkCore.Test.Entities
{
    [EntityContextMarker(typeof(ShopDbContext))]
    public class Shop:FullAuditedEntityBase
    {
        public string ShopName {  get; set; }
    }
}
