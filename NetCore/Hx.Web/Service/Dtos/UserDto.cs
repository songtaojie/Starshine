using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Web.Service.Dtos
{
    public class UserDto
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
        [Required]
        public string UserName
        {
            get; set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string PassWord
        {
            set;
            get;
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
    }
}
