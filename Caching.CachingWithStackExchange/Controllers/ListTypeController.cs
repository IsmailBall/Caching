using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string ListKey = "list";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(1);
        }

        [HttpGet]
        public IActionResult SetListCache()
        {

            _database.ListRightPush(ListKey, "Ali");
            _database.ListRightPush(ListKey, "Ahmet");
            _database.ListRightPush(ListKey, "Arslan");
            return Ok();

        }

        [HttpGet]
        public IActionResult GetListCache()
        {

            List<string> names = new();

            if (_database.KeyExists(ListKey))
            {
                _database.ListRange(ListKey).ToList().ForEach(name => names.Add(name));
            }

            return Ok();

        }

        [HttpGet]
        public IActionResult DeleteListCache(string key)
        {
            if (_database.KeyExists(ListKey))
            {
                _database.ListRemove(ListKey, key);
                _database.ListLeftPop(ListKey);
            }

            return Ok();

        }
    }
}
