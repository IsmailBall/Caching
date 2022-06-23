using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly RedisService _redis;

        public HomeController(RedisService redis)
        {
            _redis = redis;
        }

        [HttpGet("Home")]
        public IActionResult Index()
        {
            _redis.GetDatabase(1);
            string result = _redis.Database == null ? "true" : "false"; 
            return Ok(result);
        }
    }
}
