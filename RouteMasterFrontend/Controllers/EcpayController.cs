using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RouteMasterFrontend.Controllers
{
    public class EcpayController : Controller
    {
        private readonly RouteMasterContext _context;
        public EcpayController(RouteMasterContext context)
        {
            _context = context;
        }
    
        public IActionResult Index()
        {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = "https://localhost:7145/";

            var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name)?.Id;
           

            var order = new Dictionary<string, string>
            {
                { "MerchantID", "3002607" }, // 請填入你的特店編號
                { "MerchantTradeNo", orderId },
                { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
                { "TotalAmount", "100" },
                { "TradeDesc", "無" },
                { "ItemName", "測試商品" },
                { "ExpireDate", "3" },
                { "ReturnURL", $"{website}/api/Ecpay/AddPayInfo" },
                { "OrderResultURL", $"{website}/Ecpay/PayInfo/{orderId}" },
                { "PaymentType", "aio" },
                { "ChoosePayment", "ALL" },
                { "EncryptType", "1" }
            };

            order["CheckMacValue"] = GetCheckMacValue(order);

            return View(order);
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            var hashKey = "pwFHCqoQZGmho4w6"; // 請填入你的 HashKey
            var HashIV = "EkRm7iFT261dpevs";   // 請填入你的 HashIV
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
