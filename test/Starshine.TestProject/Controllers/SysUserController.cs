using Microsoft.AspNetCore.Mvc;
using Starshine.EntityFrameworkCore.Test;

namespace Starshine.TestProject.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class SysUserController : ControllerBase
    {
        

        private readonly ISysUserService _sysUserService;

        public SysUserController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            return await _sysUserService.CountAsync();
        }
    }
}
