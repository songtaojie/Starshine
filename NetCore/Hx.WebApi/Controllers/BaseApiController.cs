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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiDescriptionSettings(GroupName = "Client", Groups = new string[] { "Client" })]
    public class BaseApiController : ControllerBase
    {
    }
}
