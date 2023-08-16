using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;

namespace RouteMasterBackend.Controllers
{
    public class EcpayController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public EcpayController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public IActionResult AddPayInfo(JObject info)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) // 60 分鐘後過期
                };

                _memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, cacheEntryOptions);
                return Ok("1|OK");
            }
            catch (Exception e)
            {
                return BadRequest("0|Error");
            }
        }

        //[HttpPost]
        //public IActionResult AddAccountInfo(JObject info)
        //{
        //    try
        //    {
        //        _memoryCache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
        //        return Ok("1|OK");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest("0|Error");
        //    }
        //}
    }
}
