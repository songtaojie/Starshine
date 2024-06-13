using Hx.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.WebApi.Controllers
{
    public class RedisController : BaseApiController
    {
        private readonly ICache _cache;
        public RedisController(ICache cache)
        {
            _cache = cache;
        }

        [HttpPost(Name = "SetRedisValue")]
        public string SetRedisValue()
        {
            _cache.Set("test", "testvalue2", TimeSpan.FromSeconds(30));
            return "成功";
        }

        [HttpGet(Name = "GetRedisValue")]
        public string GetRedisValue()
        {
            var testValue = _cache.Get("test");
            return testValue;
        }

        [HttpGet(Name = "allKeys")]
        public IEnumerable<string> GetAllKeys()
        {
            return _cache.GetAllKeys();
        }
    }
}
