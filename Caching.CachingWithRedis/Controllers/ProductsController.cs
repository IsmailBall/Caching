using Caching.CachingWithRedis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Caching.CachingWithRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("SetValue")]
        public async Task<IActionResult> SetCache()
        {
            string name = "ismail";
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            await _distributedCache.SetStringAsync(nameof(name), name, distributedCacheEntryOptions);

            return Ok("Okey");
        }

        [HttpGet("GetValue")]
        public async Task<IActionResult> GetCache()
        {
            string name = "ismail";

            name = await _distributedCache.GetStringAsync(nameof(name));

            return Ok(name);
        }
        [HttpGet("SetCacheWithComplexType")]
        public async Task<IActionResult> SetCacheWithComplexType()
        {
            Product product = new Product() { Id = 1, Name = "Book", Stock = 1000 };
            
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);

            string convertedObject = JsonSerializer.Serialize(product);

            await _distributedCache.SetStringAsync("product:1", convertedObject,distributedCacheEntryOptions);

            return Ok("Okey");
        }

        [HttpGet("GetCacheWithComplexType")]
        public async Task<IActionResult> GetCacheWithComplexType()
        {
            var productJson = await _distributedCache.GetStringAsync("product:1");
            var product = JsonSerializer.Deserialize<Product>(productJson);

            return Ok(product!.Name + " " + product.Id + " " + product.Stock);
        }

        [HttpGet("SetCacheWithFile")]
        public async Task<IActionResult> SetCacheWithFile()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\ınterstller.jpg");
            var file = await System.IO.File.ReadAllBytesAsync(path);

            await _distributedCache.SetAsync("file", file);

            return Ok("Okey");
        }

        [HttpGet("GetCacheWithFile")]
        public async Task<IActionResult> GetCacheWithFile()
        {
            var fileByte = await _distributedCache.GetAsync("file");

            return File(fileByte, "image/jpg");
        }
    }
}
