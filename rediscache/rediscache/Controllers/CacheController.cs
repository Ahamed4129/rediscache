using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace rediscache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public CacheController(IDistributedCache cache)
        {
            _cache=cache;
        }
        [HttpGet("get/{key}")]
        public async Task<IActionResult> GetCache(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }
        [HttpPost("set")]
        public async Task<IActionResult> SetCache([FromBody] CacheItem item)
        {
            var options = new DistributedCacheEntryOptions()
             .SetSlidingExpiration(TimeSpan.FromMinutes(5))
             .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            await _cache.SetStringAsync(item.Key, item.Value, options);

            return Ok();
        }
        [HttpDelete("remove/{key}")]
        public async Task<IActionResult>RemoveCache(string key)
        {
            await _cache.RemoveAsync(key);
            return NoContent();
        }


    }
}
public class CacheItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}
