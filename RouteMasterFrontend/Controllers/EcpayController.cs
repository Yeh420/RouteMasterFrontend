using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Interfaces;
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
        private readonly ICoupon _coupon;

        public EcpayController(ICoupon couponservice,RouteMasterContext context)
        {
            _context = context;
            _coupon = couponservice;
        }

        public List<string> extraServiceNames=new List<string>();


        private int CalculateCartTotal(Cart cart, int couponDiscountAmount)
        {
           
            int total = 0;
            foreach (var accommodationItem in cart.Cart_AccommodationDetails)
            {
                var roomId = accommodationItem.RoomProductId;
                var room = _context.RoomProducts.FirstOrDefault(x => x.Id == roomId);

                if (room != null)
                {
                    int roomTotal = (int)room.NewPrice * accommodationItem.Quantity;
                    total += roomTotal;
                }
            }
            foreach (var extraserviceItem in cart.Cart_ExtraServicesDetails)
            {

                var extraserviceProductId = extraserviceItem.ExtraServiceProductId;
                var extraserviceProduct = _context.ExtraServiceProducts.FirstOrDefault(x => x.Id == extraserviceProductId);

                if (extraserviceProduct != null)
                {
                    int extraserviceTotal = extraserviceProduct.Price * extraserviceItem.Quantity;
                    total += extraserviceTotal;
                }
            }
            foreach (var activityItem in cart.Cart_ActivitiesDetails)
            {
                var activityProductId = activityItem.ActivityProductId;
                var activityProduct = _context.ActivityProducts.FirstOrDefault(x => x.Id == activityProductId);

                if (activityProduct != null)
                {
                    int activityTotal = activityProduct.Price * activityItem.Quantity;
                    total += activityTotal;
                }
            }
            total -= couponDiscountAmount;
            return Math.Max(total, 0);
        }

        public async Task<IActionResult> Index(int memberId, int? selectedCouponId)
        {

            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");
            var cart = _context.Carts.Where(x => x.Id == cartIdFromCookie).Include(x=>x.Cart_ActivitiesDetails).Include(x=>x.Cart_ExtraServicesDetails).Include(x=>x.Cart_AccommodationDetails).First();
            var coupons = await _coupon.GetAllCouponsAsync();
            var selectedCoupon = coupons.FirstOrDefault(c => c.Id == selectedCouponId); 
            var couponDiscountPercentage = selectedCoupon != null ? selectedCoupon.Discount : 0;
            var cartTotalBeforeDiscount = CalculateCartTotal(cart, 0); 
            var couponDiscountAmount = cartTotalBeforeDiscount * couponDiscountPercentage / 100;
            var cartTotal = cartTotalBeforeDiscount - couponDiscountAmount;
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            var website = "https://localhost:7145/";

           

            var accommodationNameArray = new List<string>();
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
            foreach (var accommodationItem in cart.Cart_AccommodationDetails)
            {
                var roomId = _context.Rooms
                    .Where(x => x.Id == accommodationItem.RoomProductId)
                    .Select(x => x.Id)
                    .FirstOrDefault();
                var accommodationName = _context.Accommodations
                    .Where(x => x.Id == roomId) .Select(x => x.Name) .FirstOrDefault();
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
                //{ "ReturnURL", $"{website}Carts/PayInfo?Id={memberId}&selectedCouponId={selectedCouponId}" },
                { "ReturnURL", $"{website}Carts/HandleReturnURL" },
                //{ "ClientBackURL", $"{website}Carts/ConfirmPayment" },
                //{ "OrderResultURL", $"{website}Carts/PayInfo?Id={memberId}&selectedCouponId={selectedCouponId}" },
                { "OrderResultURL", $"{website}Carts/ShowPaymentResult?memberId={memberId}&selectedCouponId={selectedCouponId}" },
                { "PaymentType", "aio" },
                { "ChoosePayment", "ALL" },
                { "EncryptType", "1" }
            };

            order["CheckMacValue"] = GetCheckMacValue(order);
            //EmptyCart(memberId);


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

            //_context.Carts.Remove(cart);
            //_context.SaveChanges();
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
        public IActionResult PayInfo(string? orderId)
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
