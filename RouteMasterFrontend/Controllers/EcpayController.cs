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

        public List<string> extraServiceNames=new List<string>();

        private int CalculateCartTotal(Cart cart)
        {
            int total = 0;
           

            foreach (var accommodationItem in cart.Cart_AccommodationDetails)
            {
                var roomId = _context.Rooms.Where(x => x.Id == accommodationItem.RoomProductId).First().Id;
                var RoomPrice = _context.Rooms.Where(x => x.Id == roomId).First().Price;

                int roomTotal = RoomPrice * accommodationItem.Quantity;
                total += roomTotal;
            }
            foreach (var extraserviceItem in cart.Cart_ExtraServicesDetails)
            {

                var extraservicesProductsId = _context.ExtraServices.Where(x => x.Id == extraserviceItem.ExtraServiceProductId).First().Id;
                var extraServicesId = _context.ExtraServices.Where(x => x.Id == extraservicesProductsId).First().Id;
                var extraServiceName = _context.ExtraServices.Where(x => x.Id == extraservicesProductsId).First().Name;
                var date = _context.ExtraServiceProducts.Where(X => X.Id == extraservicesProductsId).First().Date;
                var price = _context.ExtraServiceProducts.Where(x => x.Id == extraservicesProductsId).First().Price;
                var quantity = extraserviceItem.Quantity;

                int extraServicesTotal = price * quantity;
                total += extraServicesTotal;
                extraServiceNames.Add(extraServiceName);
            }
            foreach (var activityItem in cart.Cart_ActivitiesDetails)
            {
                var activityproductsId = _context.Activities.Where(x => x.Id == activityItem.ActivityProductId).First().Id;
                var activityId = _context.Activities.Where(x => x.Id == activityproductsId).First().Id;
                var activitiesName = _context.Activities.Where(X => X.Id == activityproductsId).First().Name;
                var date = _context.ActivityProducts.Where(x => x.Id == activityproductsId).First().Date;
                var startTime = _context.ActivityProducts.Where(x => x.Id == activityproductsId).First().StartTime;
                var endTime = _context.ActivityProducts.Where(x => x.Id == activityproductsId).First().EndTime;
                var price = _context.ActivityProducts.Where(x => x.Id == activityproductsId).First().Price;
                var quantity = activityItem.Quantity;

                int activityTotal = price * quantity;
                total += activityTotal;
            }
           
            return total;
        }

        public IActionResult Index()
        {

            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");
            var cart = _context.Carts.Where(x => x.Id == cartIdFromCookie).Include(x=>x.Cart_ActivitiesDetails).Include(x=>x.Cart_ExtraServicesDetails).Include(x=>x.Cart_AccommodationDetails).First();
            var cartTotal = CalculateCartTotal(cart);
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = "https://localhost:7145/";

            var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name)?.Id;

            
            var extraServiceNameArray = new List<string>();
            var activityProductNameArray = new List<string>();

            foreach (var extraserviceItem in cart.Cart_ExtraServicesDetails)
            {
                var extraservicesProductsId = _context.ExtraServices
                    .Where(x => x.Id == extraserviceItem.ExtraServiceProductId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                var extraServiceName = _context.ExtraServices
                    .Where(x => x.Id == extraservicesProductsId)
                    .Select(x => x.Name)
                    .FirstOrDefault();

                extraServiceNameArray.Add(extraServiceName); // 將每個值添加到陣列中
            }

            foreach (var activityItem in cart.Cart_ActivitiesDetails)
            {
                var activityProductsId = _context.Activities
                    .Where(x => x.Id == activityItem.ActivityProductId)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                var activityProductName = _context.Activities
                    .Where(x => x.Id == activityProductsId)
                    .Select(x => x.Name)
                    .FirstOrDefault();

                activityProductNameArray.Add(activityProductName); // 將每個值添加到陣列中
            }


            string[] extraServiceNamesArray = extraServiceNameArray.ToArray();
            string[] activityProductNamesArray = activityProductNameArray.ToArray();

            string[] allItemNamesArray = extraServiceNamesArray.Concat(activityProductNamesArray).ToArray();

            string formattedItemNames = string.Join("#", allItemNamesArray);

            var order = new Dictionary<string, string>
            {
                { "MerchantID", "3002607" },
                { "MerchantTradeNo", orderId },
                { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
                { "TotalAmount", cartTotal.ToString() },
                { "TradeDesc", "無" },
                { "ItemName", $"RouteMaster商品-{formattedItemNames}"}, 
                { "ExpireDate", "3" },
                { "ReturnURL", $"{website}/api/Ecpay/AddPayInfo/" },
                //{ "OrderResultURL", $"{website}Ecpay/PayInfo/{orderId}" },
                { "PaymentType", "aio" },
                { "ChoosePayment", "ALL" },
                { "EncryptType", "1" }
            };

            order["CheckMacValue"] = GetCheckMacValue(order);
            EmptyCart(memberId);


            return View(order);

        }

        private void EmptyCart(int? memberId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.MemberId == memberId);
            if (cart == null) return;


            var toBeDeletedAcco = _context.Cart_AccommodationDetails.Where(x => x.CartId == cart.Id);
            _context.Cart_AccommodationDetails.RemoveRange(toBeDeletedAcco);
            _context.SaveChanges();

            var toBeDeleteEXT = _context.Cart_ExtraServicesDetails.Where(x => x.CartId == cart.Id);
            _context.Cart_ExtraServicesDetails.RemoveRange(toBeDeleteEXT);
            _context.SaveChanges();

            var toBeDeleteAct = _context.Cart_ActivitiesDetails.Where(x => x.CartId == cart.Id);
            _context.Cart_ActivitiesDetails.RemoveRange(toBeDeleteAct);
            _context.SaveChanges();

            _context.Carts.Remove(cart);
            _context.SaveChanges();
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
        [HttpGet]
        public IActionResult PayInfo(string orderId)
        {
            // 根據 orderId 取得訂單資訊，例如從記憶體快取或資料庫中取得
            var orderInfo ="OrderResult";

            if (orderInfo != null)
            {
                // 在這裡可以處理付款結果，例如顯示付款結果 View，更新訂單狀態等等
                return View(orderInfo);
            }
            else
            {
                // 找不到訂單，可能需要處理錯誤情況
                return View("Error");
            }
        }
         
      
    }
}
