using Hx.WebApi.Service;
using Hx.WebApi.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Controllers
{
    public class ApplicationUserController : BaseAdminController
    {
        private readonly IApplicationUserService _service;
        public ApplicationUserController(IApplicationUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 根据博客id获取博客信息
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ApplicationUserDto> Find(string id)
        {
            return _service.Find(id);
        }
    }
}
