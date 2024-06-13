using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Service.Dtos
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

        /// <summary>
        /// 用户枚举
        /// </summary>
        public UserDtoEnum UserDtoEnum { get; set; }
    }

    /// <summary>
    /// 用户枚举
    /// </summary>
    [Description("用户枚举")]
    public enum UserDtoEnum
    { 
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 0,
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 1
    }
}
