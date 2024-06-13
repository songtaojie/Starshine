using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Controllers
{
    /// <summary>
    /// admin基础控制器
    /// </summary>
    [Route("admin/api/[controller]/[action]")]
    [ApiController]
    [ApiDescriptionSettings(GroupName = "Admin", Groups = new string[] { "Admin" })]
    public class BaseAdminController : ControllerBase
    {
    }
}
