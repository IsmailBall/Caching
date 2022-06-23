using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string SetKey = "set";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(2);
        }

        [HttpGet]
        public IActionResult SetListCache()
        {


            _database.SetAdd(SetKey, "Ali");
            _database.SetAdd(SetKey, "Veli");
            _database.SetAdd(SetKey, "Hasan");

            return Ok();
        }

        [HttpGet]
        public IActionResult GetListCache()
        {
            HashSet<string> names = new();

            if (_database.KeyExists(SetKey))
            {
                _database.SetMembers(SetKey).ToList().ForEach(name => names.Add(name));
            }


            return Ok();

        }

        [HttpGet]
        public IActionResult DeleteListCache(string key)
        {
            if (_database.KeyExists(SetKey))
            {
                _database.SetRemove(SetKey,key);

                var randomDeleted = _database.SetPop(SetKey);
            }



            return Ok();

        }
    }
}
