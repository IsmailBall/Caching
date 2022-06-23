using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class StringTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(0);
        }

        [HttpGet]
        public IActionResult SetStringCache()
        {
            _database.StringSet("name", "Ismail");
            _database.StringSet("visiter", 1000);

            var imgByteArray = default(byte[]);
            _database.StringSet("image", imgByteArray);


            return Ok();
            
        }

        [HttpGet]
        public IActionResult GetStringCache()
        {
            var value = _database.StringGet("name");

            var rangedValue = _database.StringGetRange("name", 0, 3);
            _database.StringAppend("name", " Ismail");

            var increasedVisiter = _database.StringIncrement("visiter", 10);
            var decreasedVisiter = _database.StringDecrement("visiter", 10);

            return value.HasValue ? Ok(value.ToString()) : Ok("NO Value");

        }
    }
}
