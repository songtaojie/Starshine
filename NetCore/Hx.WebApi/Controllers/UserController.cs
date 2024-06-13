using Hx.WebApi.Controllers;
using Hx.WebApi.Service;
using Hx.WebApi.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        [HttpPost]
        public Task<UserDto> Find(string id)
        {
            return _userService.Find(id);
        }

        /// <summary>
        /// 测试分库
        /// 获取IdentityServer数据库中的用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApplicationUserDto> FindApplicationUser(string id)
        {
            return await _userService.FindApplicationUser(id);
        }
        
    }
}
