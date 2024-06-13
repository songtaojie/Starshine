using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Service.Dtos
{
    public class ApplicationUserDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get; set;
        }
    }
}
