using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string SetKey = "hashset";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(4);
        }

        [HttpGet]
        public IActionResult SetListCache()
        {

            _database.HashSet(SetKey, "red", "kirmizi");
            _database.HashSet(SetKey, "blue", "mavi");
            _database.HashSet(SetKey, "green", "yesil");
            
            _database.KeyExpire(SetKey, DateTime.Now.AddMinutes(2));

            return Ok();
        }

        [HttpGet]
        public IActionResult GetListCache()
        {
            Dictionary<string,string> colors = new();

            if (_database.KeyExists(SetKey))
            {
                _database.HashGetAll(SetKey).ToList().ForEach(entry =>
                {
                    colors.Add(entry.Name, entry.Value);
                });
            }


            return Ok();

        }

        [HttpGet]
        public IActionResult DeleteListCache(string key)
        {
            if (_database.KeyExists(SetKey))
            {
                _database.HashDelete(SetKey, key);
            }

            return Ok();

        }
    }
}
