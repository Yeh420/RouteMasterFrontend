using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.ViewModels.Carts;
using static RouteMasterFrontend.Models.ViewModels.Carts.Cart_ExtraServiceDetailsVM;

namespace RouteMasterFrontend.Controllers
{
    public class CartsController : Controller
    {
        private readonly RouteMasterContext _context;

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
                // 查詢對應的 ExtraServiceProduct
                var extraServiceProduct = _context.ExtraServiceProducts
                    .FirstOrDefault(p => p.Id == extraserviceId);

                if (extraServiceProduct != null)
                {
                    //// 查詢現有的購物車
                    //var memberId = GetMemberIdByAccount(User.Identity.Name);
                    //var cart = _context.Carts
                    //    .Include(c => c.Cart_ExtraServicesDetails) // 可能需要根據您的資料庫結構進行調整
                    //    .FirstOrDefault(c => c.MemberId == memberId);

                    //if (cart == null)
                    //{
                    //    // 建立新的購物車
                    //    cart = new Cart { MemberId = memberId };
                    //    _context.Carts.Add(cart);
                    //    _context.SaveChanges();
                    //}



                    // 建立新的 CartItem
                    var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["CartId"] ?? "0");
                    var cartItem = new Cart_ExtraServicesDetail
                    {
                        CartId = cartIdFromCookie,
                        ExtraServiceProductId = extraserviceId,
                        Quantity = 1
                    };


                    _context.Cart_ExtraServicesDetails.Add(cartItem);
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
        public IActionResult RemoveExtraServiceFromCart(int extraserviceId)
        {
            try
            {
                var cartItem = _context.Cart_ExtraServicesDetails.FirstOrDefault(p => p.Id == extraserviceId);
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
        
        public IActionResult AddActivitiesDetail2Cart(int activitiesId)
        {
            try
            {
                var activitiesProduct = _context.ActivityProducts.FirstOrDefault(p => p.Id == activitiesId);
                if (activitiesProduct != null)
                {
                    var cartItem = new Cart_ActivitiesDetail
                    {
                        CartId = 1,
                        ActivityProductId = activitiesProduct.Id,
                        Quantity = 1
                    };
                    _context.Cart_ActivitiesDetails.Add(cartItem);
                    _context.SaveChanges();
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
                // Handle the case where member is not found
                return RedirectToAction("Index", "Home"); // Redirect to a suitable action
            }

            var cart = GetCartInfo(memberId.Value);

            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() || cart.Cart_ActivitiesDetails.Any() || cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {
                ViewBag.ErrorMessage = "購物車是空的，無法進行結帳";
            }

            ViewBag.AllowCheckout = allowCheckout;
            var cartItems = new List<object>();

            if (cart.Cart_ExtraServicesDetails != null)
            {
                cartItems.AddRange(cart.Cart_ExtraServicesDetails);
            }

            if (cart.Cart_ActivitiesDetails != null)
            {
                cartItems.AddRange(cart.Cart_ActivitiesDetails);
            }

            if (cart.Cart_AccommodationDetails != null)
            {
                cartItems.AddRange(cart.Cart_AccommodationDetails);
            }

            ViewBag.CartItems = cartItems;

            return View(cartItems);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckoutPost()
        {
            var memberId = _context.Members.FirstOrDefault(m => m.Account == User.Identity.Name)?.Id;
            if (memberId == null)
            {
                // Handle the case where member is not found
                return RedirectToAction("Index", "Home"); // Redirect to a suitable action
            }

            var cart = GetCartInfo(memberId.Value);

            bool allowCheckout = cart.Cart_ExtraServicesDetails.Any() || cart.Cart_ActivitiesDetails.Any() || cart.Cart_AccommodationDetails.Any();

            if (!allowCheckout)
            {
                ModelState.AddModelError(string.Empty, "購物車是空的，無法進行結帳");
                return View();
            }

            ProcessCheckout(memberId.Value); // 呼叫處理結帳的方法

            return View("ConfirmCheckout");
        }

        private void ProcessCheckout(int memberId)
        {
            // 建立訂單主檔明細檔
            CreateOrder(memberId);

            // 清空購物車
            EmptyCart(memberId);
        }

        private void CreateOrder(int memberId)
        {
            // 根據您的需求，實現建立訂單主檔和明細檔的邏輯
            // 使用 memberId 和其他相關資訊建立訂單
        }
        private void EmptyCart(int memberId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.MemberId == memberId);
            if (cart == null) return;

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
