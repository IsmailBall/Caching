using Caching.CachingWithStackExchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Caching.CachingWithStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortedSetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private readonly string SetKey = "sortedset";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDatabase(3);
        }

        [HttpGet]
        public IActionResult SetListCache()
        {


            var entry1 = new SortedSetEntry("Ali", 10);
            var entry2 = new SortedSetEntry("Veli", 100);
            var entry3 = new SortedSetEntry("Hasan", 1000);
            var entries = new SortedSetEntry[] { entry1, entry2, entry3 };

            _database.SortedSetAdd(SetKey, values: entries);

            _database.KeyExpire(SetKey, DateTime.Now.AddMinutes(2));

            return Ok();
        }

        [HttpGet]
        public IActionResult GetListCache()
        {
            HashSet<string> names = new();

            if (_database.KeyExists(SetKey))
            {
                _database.SortedSetScan(SetKey).ToList().ForEach(element => names.Add(element.Element + " " + element.Score.ToString()));

                _database.SortedSetRangeByRank(SetKey, order: Order.Descending).ToList().ForEach(x => Console.WriteLine(x.ToString()));
            }


            return Ok();

        }

        [HttpGet]
        public IActionResult DeleteListCache(string key)
        {
            if (_database.KeyExists(SetKey))
            {
                _database.SortedSetRemove(SetKey, key);
            }

            return Ok();

        }
    }
}
