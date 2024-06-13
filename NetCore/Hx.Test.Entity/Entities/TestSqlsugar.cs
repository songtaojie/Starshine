using Hx.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Test.Entity.Entities
{
    [SugarTable(null,"测试表")]
    public class TestSqlsugar:EntityBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsNullable =true,Length =200,ColumnDescription ="名称")]
        public string Name { get; set; }
    }
}
