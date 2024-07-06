using Microsoft.AspNetCore.Mvc;
using Starshine.EntityFrameworkCore.Test;
using Starshine.EntityFrameworkCore.Test.Services;

namespace Starshine.TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {

        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            return await _shopService.CountAsync();
        }
    }
}
