using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Shared.Components.ExtraServicesDetails2
{
    public class ExtraServicesDetails2ViewComponent : ViewComponent
    {
        private readonly RouteMasterContext _context;
        public ExtraServicesDetails2ViewComponent(RouteMasterContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IViewComponentResult Invoke(int cartid)
        {
            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");

            // 將讀取的值存入 ViewData
            ViewData["CartId"] = cartIdFromCookie;



            var cart = _context.Cart_ExtraServicesDetails
                .Where(c => c.CartId == cartid)
                .Include(c => c.ExtraServiceProduct)
                .Include(c => c.ExtraServiceProduct.ExtraService) // Load the ExtraService within ExtraServiceProduct
                .ToList(); ;
            // 使用 View 屬性設定要回傳的檢視名稱

            if (cart.Any())
            {
                return View("ExtraServicesDetailsPartialView", cart);
            }
            else
            {
                return Content("");
            }
        }
    }
}
