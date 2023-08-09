using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using ECPay.Payment.Integration;


namespace RouteMasterFrontend.Controllers
{
    
    public class CartsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly RouteMasterContext _context;
        private const string MerchantID = "3002607";
        private const string PaymentApiUrl = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
        private const string HashKey = "pwFHCqoQZGmho4w6"; 
        private const string HashIV = "EkRm7iFT261dpevs";
        public CartsController(RouteMasterContext context)
        {
            _context = context;
        }
      
        // GET: Carts
        public async Task<IActionResult> Index()
        {
            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");

            // 將讀取的值存入 ViewData
            ViewData["CartId"] = cartIdFromCookie;
            var routeMasterContext = _context.Carts.Include(c => c.Member);
            return View(await routeMasterContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult IndexGET()
        {
            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");

            // 將讀取的值存入 ViewData
            ViewData["CartId"] = cartIdFromCookie;

            return View();
        }

        public IActionResult Info()
		{
			var customerAccount = User.Identity.Name;
			int memberId = GetMemberIdByAccount(customerAccount);
			Cart cart = GetCartInfo(memberId);
			return View(cart);
		}

        //public IActionResult Add2Cart(int extraserviceId, string customerAccount)
        //{
        //    var memberId = GetMemberIdByAccount(customerAccount);
        //    var cart = _context.Carts.SingleOrDefault(c=>c.MemberId == memberId);
        //    if (cart == null)
        //    {
        //        cart = new Cart { MemberId = memberId };
        //        _context.Carts.Add(cart);
        //        _context.SaveChanges();
        //    }
        //    ViewBag.cartid = cart.Id;


        //    return Json(new { success = true, message = "已加入購物車", cartId = cart.Id });
        
        //}
		
        public IActionResult ExtraServicesDetailsPartialView(int memberId)
        {
            var extraServicesDetails = _context.Cart_ExtraServicesDetails
                   //.Where(c => c.CartId == memberId)
                   .Include(c => c.ExtraServiceProduct) // Load the ExtraServiceProduct
                   .Include(c => c.ExtraServiceProduct.ExtraService) // Load the ExtraService within ExtraServiceProduct
                   .ToList();

            //List<Cart_ExtraServiceDetailsVM> viewModelList = extraServicesDetails.Select(cartExtraServiceDetail => new Cart_ExtraServiceDetailsVM
            //{
            //    Id = cartExtraServiceDetail.Id,
            //    CartId = cartExtraServiceDetail.CartId,
            //    ExtraServiceProductId = cartExtraServiceDetail.ExtraServiceProductId,
            //    Quantity = cartExtraServiceDetail.Quantity,
            //    ExtraServicePrice= cartExtraServiceDetail.ExtraServiceProduct.Price,
            //    ExtraServiceName = cartExtraServiceDetail.ExtraServiceProduct.ExtraService.Name // Assuming you have ExtraServiceName property in Cart_ExtraServicesDetail entity   
            //}).ToList();

            return PartialView("ExtraServicesDetailsPartialView");
        }
        public List<Cart_ExtraServicesDetail> GetCartExtraServicesDetails(int memberId)
		{


			// 使用 EF Core 的資料庫上下文查詢 Cart_ExtraServicesDetails 資料
			var extraServicesDetails = _context.Cart_ExtraServicesDetails
				.Where(c => c.Cart.MemberId == memberId)
				.ToList();

			return extraServicesDetails;
		}
        public IActionResult AddExtraService2Cart(int extraserviceId)
        {
            try
            {
               
                var extraServiceProduct = _context.ExtraServiceProducts
                    .FirstOrDefault(p => p.Id == extraserviceId);

                if (extraServiceProduct != null)
                {
                    
                    var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["CartId"] ?? "0");
                    var cartItem = new Cart_ExtraServicesDetail
                    {
                        CartId = cartIdFromCookie,
                        ExtraServiceProductId = extraserviceId,
                        Quantity = 1
                    };


                    _context.Cart_ExtraServicesDetails.Add(cartItem);
                    _context.SaveChanges();
                    Response.Cookies.Append("CartId", cartIdFromCookie.ToString());


                    return Json(new { success = true, message = "Successfully added to cart." });
                }
                else
                {
                   
                    return Json(new { success = false, message = "Product not found." });
                }
            }
            catch
            {
                // 加入購物車失敗時回傳 JSON 物件
                return Json(new { success = false, message = "Failed to add to cart." });
            }
        }
        public IActionResult RemoveExtraServiceFromCart(int extraserviceId)
        {
            try
            {
                var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["CartId"] ?? "0");
                var cartItem = _context.Cart_ExtraServicesDetails.FirstOrDefault(p => p.CartId == cartIdFromCookie && p.Id == extraserviceId);
                if (cartItem != null)
                {
                    _context.Cart_ExtraServicesDetails.Remove(cartItem);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Successfully removed from cart.", extraserviceId = extraserviceId });
                }
                else
                {
                    return Json(new { success = false, message = "Item not found in cart." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Failed to remove from the cart." });
            }

        }


        public IActionResult ActivitiesDetailsPartialView(int memberId)
        {
            var activitiesServicesDetails = _context.Cart_ActivitiesDetails
                .Include(c => c.ActivityProduct)
                .Include(c => c.ActivityProduct.Activity)
                .ToList();
            return PartialView("ActivitiesDetailsPartialView");
        }
        public List<Cart_ActivitiesDetail>GetCartActivities(int memberId)
        {
            var activitiesDetails= _context.Cart_ActivitiesDetails
                .Where(c=>c.Cart.MemberId==memberId)
                .ToList() ;
            return activitiesDetails;
        }

        public IActionResult AccommodationDetailsPartialView(int memberId)
        {
            var accommodationDetails = _context.Cart_AccommodationDetails
                .Include(c => c.RoomProduct)
                .Include(c => c.RoomProduct.Room.Accommodation)
                .ToList();
            return PartialView("AccommodationDetailsPartialView");
        }
        public List<Cart_AccommodationDetail>GetCartAccommodationDetails(int memberId)
        {
            var accomodationDetails = _context.Cart_AccommodationDetails
                .Where(c => c.CartId == memberId)
                .ToList();
            return accomodationDetails;
        }

        [HttpPost]
        public IActionResult AddAccommodation2Cart( int roomProductId)
        {
            try
            {
                var RoomProduct = _context.RoomProducts.FirstOrDefault(r=>r.Id == roomProductId);
                if(RoomProduct != null)
                {
                    var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["cartId"] ?? "0");
                    //var cartIdFromCookie = 3;
                    var cartItem = new Cart_AccommodationDetail
                    {
                        Id = 0,
                        CartId = cartIdFromCookie,
                        RoomProductId = RoomProduct.Id,
                        Quantity = 1
                    };
                    _context.Cart_AccommodationDetails.Add(cartItem);
                    _context.SaveChanges();
                    Response.Cookies.Append("cartId", cartIdFromCookie.ToString());
                    return Json(new { success = true, message = "Successfully added to cart" });
                }
                else
                {
                    return Json(new { success = false, message = "Product not found." });
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Failed to add to cart."+ex });
            }
        }
        public IActionResult RemoveAccommodationFromCart(int accommodationId)
        {
            try
            {
                var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["cartId"] ?? "0");
                var cartItem=_context.Cart_AccommodationDetails.FirstOrDefault(x=>x.CartId==cartIdFromCookie&& x.Id==accommodationId);
                if (cartItem != null)
                {
                    _context.Cart_AccommodationDetails.Remove(cartItem);
                    _context.SaveChanges();
                    return Json(new {success=true, message="Successfully removed from cart.", accommodationId=accommodationId});
                }
                else
                {
                    return Json(new { success = false, message = "Item not found in Cart." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Failed to remove from the cart." });
            }
        }
        public IActionResult AddActivitiesDetail2Cart(int activitiesId)
        {
            try
            {
                var activitiesProduct = _context.ActivityProducts.FirstOrDefault(p => p.Id == activitiesId);
                if (activitiesProduct != null)
                {
                    var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["cartId"] ?? "0");
                    var cartItem = new Cart_ActivitiesDetail
                    {
                        CartId = cartIdFromCookie,
                        ActivityProductId = activitiesProduct.Id,
                        Quantity = 1
                    };
                    _context.Cart_ActivitiesDetails.Add(cartItem);
                    _context.SaveChanges();

                    Response.Cookies.Append("cartId", cartIdFromCookie.ToString());
                    return Json(new { success = true, message = "Successfully added to cart." });
                }
                else
                {
                    return Json(new { succes = false, message = "Product not found." });
                }
            }
            catch
            {
                // 加入購物車失敗時回傳 JSON 物件
                return Json(new { success = false, message = "Failed to add to cart." });
            }

		
        }
      
        public IActionResult RemoveActivitiesFromCart(int activitiesId)
        {
            try
            {
                var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["cartId"] ?? "0");
                var cartItem = _context.Cart_ActivitiesDetails.FirstOrDefault(x=>x.CartId== cartIdFromCookie&& x.Id == activitiesId);
                if(cartItem != null)
                {
                    _context.Cart_ActivitiesDetails.Remove(cartItem);
                    _context.SaveChanges();

                    return Json(new {success=true, message="Successfully  removed from cart.", activitiesId=activitiesId});
                }
                else
                {
                    return Json(new { sucess = false, message = "Item not found in cart." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Failed to remove from the cart." });
            }
        }

        public IActionResult RefreshCart(int memberId)
        {
           

            //ViewData["CartId"] = _context.Carts.Where(s => s.MemberId == memberId).First().Id;
            return ViewComponent("CartPartial");
        }

        //public IActionResult Checkout()
        //{
        //    var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name).Id;
        //    var cart = GetCartInfo(memberId);

        //    if (cart.AllowCheckout == false) ViewBag.ErrorMessage = "購物車是空的,無法進行結帳";

        //    return View();
        //}
        [HttpGet]
        public ActionResult Checkout()
        {
            var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name)?.Id;
            if (memberId == null)
            {
                
                return RedirectToAction("Index", "Home"); 
            }

            var cart = GetCartInfo(memberId.Value);

            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() || cart.Cart_ActivitiesDetails.Any() || cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {
                ViewBag.ErrorMessage = "購物車是空的，無法進行結帳";
            }
            var member = _context.Members.FirstOrDefault(m => m.Id == memberId.Value);

            if (member != null)
            {
                ViewBag.MemberFirstName = member.FirstName;
                ViewBag.MemberLastName = member.LastName;
                ViewBag.CellPhoneNumber = member.CellPhoneNumber;
                ViewBag.Address = member.Address;
            }

            ViewBag.PaymentMethods = _context.PaymentMethods.ToList();
            List<Cart> cartList = new List<Cart> { cart };

            return View(cartList);
        }

     
        [HttpPost]
        public ActionResult CheckoutPost(string paymentmethod, string name, string telephone, string address, string?note)
        {
            var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name).Id;
            if (memberId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cart = GetCartInfo(memberId);
            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() || cart.Cart_ActivitiesDetails.Any() || cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {
                ModelState.AddModelError(string.Empty, "購物車是空的，無法進行結帳");
                return View("Checkout");
            }
            ProcessCheckout(memberId, note);

            return RedirectToAction("Index", "Ecpay");
        }

      
        private void ProcessCheckout(int memberId, string?note)
        {
            var cart = GetCartInfo(memberId);

            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() ||
                                 cart.Cart_ActivitiesDetails.Any() ||
                                 cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {
                // 購物車是空的，無法進行結帳，進行相應處理
                return;
            }

            // 創建新訂單
            CreateOrder(memberId, note);
            //GetPaymentRedirectUrl(order);

            EmptyCart(memberId);

        }

        private void CreateOrder(int memberId, string note)
        {
            var cart = GetCartInfo(memberId);

            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() ||
                                 cart.Cart_ActivitiesDetails.Any() ||
                                 cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {

                return;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    int cartTotal = CalculateCartTotal(cart);
                    var order = new Order
                    {
                        MemberId = memberId,
                        PaymentMethodId = 1,
                        PaymentStatusId = 1,
                        OrderHandleStatusId = 1,
                        CouponsId = 1,
                        CreateDate = DateTime.Now,
                        ModifiedDate = null,
                        Total = cartTotal


                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    foreach (var accommodationItem in cart.Cart_AccommodationDetails)
                    {

                        var roomId = _context.Rooms.Where(x => x.Id == accommodationItem.RoomProductId).First().Id;
                        var accommodationId = _context.Rooms.Where(x => x.Id == roomId).First().AccommodationId;

                        var accommodationName = _context.Accommodations.Where(x => x.Id == accommodationId).First().Name;
                        var RoomType = _context.Rooms.Where(x => x.Id == roomId).First().RoomTypeId;
                        var RoomName = _context.Rooms.Where(x => x.Id == roomId).First().Name;
                        var RoomPrice = _context.Rooms.Where(x => x.Id == roomId).First().Price;
                        var Quantity = _context.Rooms.Where(x => x.Id == roomId).First().Quantity;
                        TimeSpan checkinTimeSpan = _context.Accommodations.Where(x => x.Id == accommodationId).First().CheckIn ?? TimeSpan.Zero;
                        var Checkin = DateTime.Today.Add(checkinTimeSpan);
                        TimeSpan checkoutTimeSpan = _context.Accommodations.Where(x => x.Id == accommodationId).First().CheckOut ?? TimeSpan.Zero;
                        var Checkout = DateTime.Today.Add(checkoutTimeSpan);

                        var accommodationDetail = new OrderAccommodationDetail
                        {
                            OrderId = order.Id,
                            AccommodationId = accommodationId,
                            AccommodationName = accommodationName,
                            RoomProductId = accommodationItem.RoomProductId,
                            RoomType = RoomType.ToString(),
                            RoomName = RoomName,
                            CheckIn = Checkin,
                            CheckOut = Checkout,
                            RoomPrice = RoomPrice,
                            Quantity = Quantity,
                            Note = note
                        };

                        //var paymentClient = new Models.Infra.ECPayPaymentClient(
                        //   apiUrl: "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                        //   hashKey: "pwFHCqoQZGmho4w6",
                        //   hashIV: "EkRm7iFT261dpevs",
                        //   merchantID: "3002607"
                        //            );

                        //// 呼叫綠界支付 API
                        //var paymentResult = paymentClient.CallECPayApiAsync().Result;
                        //_context.OrderAccommodationDetails.Add(accommodationDetail);
                        _context.SaveChanges();
                    }


                    transaction.Commit();
                   
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException;
                    transaction.Rollback();
                }
            }
        }

        //[HttpPost]
        //public IActionResult ReturnUrl([FromForm] Dictionary<string, string> formData)
        //{
        //    string merchantTradeNo = Guid.NewGuid().ToString("N").Substring(0, 20);
        //    string tradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //    string paymentType = "aio";
        //    string tradeDesc = "Sample Transaction";
        //    string itemNames = "Item1#Item2";
        //    string returnURL = "https://yourwebsite.com/payment/callback";
        //    string choosePayment = "All";

        //    string checkMacValue = $"HashKey={HashKey}&MerchantID={MerchantID}&MerchantTradeNo={merchantTradeNo}&PaymentDate={tradeDate}&PaymentType={paymentType}&TotalAmount={cartTotal}&TradeDesc={tradeDesc}&ItemName={itemNames}&ReturnURL={returnURL}&ChoosePayment={choosePayment}&CheckMacValue={HashIV}";

        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(checkMacValue));
        //        string checkMac = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

        //        string queryString = HttpUtility.UrlEncode($"MerchantID={MerchantID}&MerchantTradeNo={merchantTradeNo}&MerchantTradeDate={tradeDate}&PaymentType={paymentType}&TotalAmount={cartTotal}&TradeDesc={tradeDesc}&ItemName={itemNames}&ReturnURL={returnURL}&ChoosePayment={choosePayment}&CheckMacValue={checkMac}&EncryptType=1");

        //        var redirectUrl = $"{PaymentApiUrl}?{queryString}";

        //        // 使用 JsonResult 返回 JSON 數據
        //        return new JsonResult(new { redirectUrl });
        //    }
        //}

      


        //    private void PrepareAndExecutePayment(Order order)
        //    {
        //        var paymentParameters = new Dictionary<string, string>
        //{
        //            { "MerchantID", "3002607" },
        //            { "MerchantTradeNo", order.Id.ToString() },
        //            { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
        //            { "PaymentType", "aio" },
        //            { "TotalAmount", order.Total.ToString() },

        //        };
        //        var checkMacValue = BuildCheckMacValue(paymentParameters);
        //        paymentParameters.Add("CheckMacValue", checkMacValue);

        //        var content = new FormUrlEncodedContent(paymentParameters);
        //        using (var client = new HttpClient())
        //        {
        //            var response = client.PostAsync(PaymentApiUrl, content).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                Console.WriteLine("Payment successful");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Payment failed");
        //            }
        //        }

        //    }

        //    private string BuildCheckMacValue(Dictionary<string, string> parameters)
        //    {
        //        string szCheckMacValue = String.Format("HashKey={0}{1}&HashIV={2}",
        //            HashKey, string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}")), HashIV);
        //        szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();

        //        using (SHA256 sha256 = SHA256Managed.Create())
        //        {
        //            byte[] bytes = Encoding.UTF8.GetBytes(szCheckMacValue);
        //            byte[] hashBytes = sha256.ComputeHash(bytes);
        //            szCheckMacValue = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //        }

        //        return szCheckMacValue;
        //    }

        //    private string GetPaymentRedirectUrl(Order order)
        //    {
        //        string PaymentBaseUrl = "https://localhost:7145/Carts/CheckoutPost";
        //        var paymentUrl = PaymentBaseUrl + "?orderId=" + order.Id +
        //            "&totalAmount=" + order.Total +
        //            "&memberId=" + order.MemberId +
        //            "&paymentMethodId=" + order.PaymentMethodId +
        //            "&paymentStatusId=" + order.PaymentStatusId +
        //            "&orderHandleStatusId=" + order.OrderHandleStatusId +
        //            "&couponsId=" + order.CouponsId +
        //            "&createDate=" + order.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") +
        //            "&modifiedDate=" + (order.ModifiedDate.HasValue ? order.ModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
        //        return paymentUrl;
        //    }

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
           

            return total;
        }

        //private void CreateOrder(int memberId, Cart cart)
        //{
        //    var order = new Order
        //    {
        //        MemberId = memberId,
        //        TravelPlanId = cart..TravelPlanId
        //        Total = cart.Total,
        //        CreateDate = DateTime.Now,

        //    };
        //}

        //public void CreateOrder(int memberId, int travelPlanId, int paymentMethodId, int couponsId, List<ord> accommodationItems, List<ActivityOrderItem> activityItems, List<ExtraServiceOrderItem> extraServiceItems)
        //{

        //        var order = new Order
        //        {
        //            MemberId = memberId,
        //            TravelPlanId = travelPlanId,
        //            PaymentMethodId = paymentMethodId,
        //            PaymentStatusId = initialPaymentStatusId, // Set the initial payment status ID
        //            OrderHandleStatusId = initialHandleStatusId, // Set the initial handle status ID
        //            CouponsId = couponsId,
        //            CreateDate = DateTime.Now,
        //            ModifiedDate = null, // Set this as needed
        //            Total = CalculateTotal(accommodationItems, activityItems, extraServiceItems)
        //        };

        //        _context.Orders.Add(order);
        //        _context.SaveChanges();

        //        int orderId = order.Id;

        //        foreach (var accommodationItem in accommodationItems)
        //        {
        //            var accommodationDetail = new OrderAccommodationDetail
        //            {
        //                OrderId = orderId,
        //                AccommodationId = accommodationItem.AccommodationId,
        //                AccommodationName = accommodationItem.AccommodationName,
        //                RoomProductId = accommodationItem.RoomProductId,
        //                RoomType = accommodationItem.RoomType,
        //                RoomName = accommodationItem.RoomName,
        //                CheckIn = accommodationItem.CheckIn,
        //                CheckOut = accommodationItem.CheckOut,
        //                RoomPrice = accommodationItem.RoomPrice,
        //                Quantity = accommodationItem.Quantity,
        //                Note = accommodationItem.Note
        //            };

        //           _context.OrderAccommodationDetails.Add(accommodationDetail);
        //        }

        //        foreach (var activityItem in activityItems)
        //        {
        //            var activityDetail = new OrderActivitiesDetail
        //            {
        //                OrderId = orderId,
        //                ActivityId = activityItem.ActivityId,
        //                ActivityName = activityItem.ActivityName,
        //                ActivityProductId = activityItem.ActivityProductId,
        //                Date = activityItem.Date,
        //                StartTime = activityItem.StartTime,
        //                EndTime = activityItem.EndTime,
        //                Price = activityItem.Price,
        //                Quantity = activityItem.Quantity
        //            };

        //        _context.OrderActivitiesDetails.Add(activityDetail);
        //        }

        //        foreach (var extraServiceItem in extraServiceItems)
        //        {
        //            var extraServiceDetail = new OrderExtraServicesDetail
        //            {
        //                OrderId = orderId,
        //                ExtraServiceId = extraServiceItem.ExtraServiceId,
        //                ExtraServiceName = extraServiceItem.ExtraServiceName,
        //                ExtraServiceProductId = extraServiceItem.ExtraServiceProductId,
        //                Date = extraServiceItem.Date,
        //                Price = extraServiceItem.Price,
        //                Quantity = extraServiceItem.Quantity
        //            };

        //        _context.OrderExtraServicesDetails.Add(extraServiceDetail);
        //        }

        //        _context.SaveChanges();

        //}

        //private int CalculateTotal(List<AccommodationOrderItem> accommodationItems, List<ActivityOrderItem> activityItems, List<ExtraServiceOrderItem> extraServiceItems)
        //{
        //    // Calculate and return the total amount based on the items in the order
        //    // You need to implement the logic for calculating the total based on your requirements
        //}


        private void EmptyCart(int memberId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.MemberId == memberId);
            if (cart == null) return;


            var toBeDeletedAcco = _context.Cart_AccommodationDetails.Where(x => x.CartId == cart.Id);
            _context.Cart_AccommodationDetails.RemoveRange(toBeDeletedAcco);
            _context.SaveChanges();

            var toBeDeleteEXT = _context.Cart_ExtraServicesDetails.Where(x=>x.CartId== cart.Id);
            _context.Cart_ExtraServicesDetails.RemoveRange(toBeDeleteEXT);
            _context.SaveChanges();

            var toBeDeleteAct = _context.Cart_ActivitiesDetails.Where(x=>x.CartId==cart.Id);
            _context.Cart_ActivitiesDetails.RemoveRange(toBeDeleteAct);
            _context.SaveChanges();




            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }

        public IActionResult AddAccomodation2Cart(int accomodationId)
        {

            try
            {
                var RoomProduct = _context.RoomProducts.FirstOrDefault(p => p.Id == accomodationId);
                if(RoomProduct!= null)
                {
                    var cartItem = new Cart_AccommodationDetail
                    {
                        CartId = 1,
                        RoomProductId = RoomProduct.Id,
                        Quantity = 1
                    };
                    _context.Cart_AccommodationDetails.Add(cartItem);
                    _context.SaveChanges();
					// 加入購物車成功後回傳 JSON 物件
					return Json(new { success = true, message = "Successfully added to cart." });
				}
				else
				{
					// 找不到對應的 ExtraServiceProduct，回傳錯誤訊息
					return Json(new { success = false, message = "Product not found." });
				}
			}
			catch
			{
				// 加入購物車失敗時回傳 JSON 物件
				return Json(new { success = false, message = "Failed to add to cart." });
			}
		
            
        }
        private int GetMemberIdByAccount(string Account)
		{

            if (!User.Identity.IsAuthenticated)
            {
              
                return -1;
            }
            var member = _context.Members
				.Where(m => m.Account == Account)
				.FirstOrDefault();
			if (member != null)
			{
				return member.Id;
			}
			else
			{
				return -1;
			}
		}
        private int GetCartId()
        {
            var userClaims = HttpContext.User.Claims;
            var userAccountClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (userAccountClaim != null)
            {
                string userAccount = userAccountClaim.Value;

                var memberId = GetMemberIdByAccount(userAccount);
                var cart = GetCartInfo(memberId);

                return cart.Id;
            }

          
            return -1;
        }

        public Cart GetCartInfo(int memberId)
        {
            var cart = _context.Carts
                .Include(c => c.Cart_ExtraServicesDetails)
                .Include(c => c.Cart_ActivitiesDetails)
                .Include(c => c.Cart_AccommodationDetails)
                .Where(c => c.MemberId == memberId)
                .FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart { MemberId = memberId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            return cart;

        }
        // GET: Carts/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", cart.MemberId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", cart.MemberId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", cart.MemberId);
            return View(cart);
        }

        //// GET: Carts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Carts == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Carts
        //        .Include(c => c.Member)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cart);
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Carts == null)
        //    {
        //        return Problem("Entity set 'RouteMasterContext.Carts'  is null.");
        //    }
        //    var cart = await _context.Carts.FindAsync(id);
        //    if (cart != null)
        //    {
        //        _context.Carts.Remove(cart);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CartExists(int id)
        {
          return (_context.Carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


       
    }
}
