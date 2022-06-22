using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCacheApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            MemoryCacheEntryOptions memoryCacheEntryOptions = new();

            memoryCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            memoryCacheEntryOptions.SlidingExpiration = TimeSpan.FromSeconds(2);

            memoryCacheEntryOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"key => {key}:  value: {value} : reason: {reason}");
            });

            _memoryCache.Set<string>("time", DateTime.Now.ToString(),memoryCacheEntryOptions);
            return Ok();
        }
        [HttpGet("GetResult")]
        public IActionResult GetResult()
        {

            
            var result = _memoryCache.GetOrCreate("time", (entry) =>
            {
                var value = DateTime.Now.ToString();
                entry.SetValue(value);
                return value;
            });
            

            var data = _memoryCache.Get<string>("callback");
            
            return Ok(data);
        }
    }
}
