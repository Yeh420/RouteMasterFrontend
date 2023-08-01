using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Carts.Components.ExtraServicesDetails
{
    public class ExtraServicesDetailsViewComponent : ViewComponent
    {
        private readonly RouteMasterContext _context;
        public ExtraServicesDetailsViewComponent(RouteMasterContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int cartid)
        {
            // 假設您有一個用來查詢 Cart 的 ExtraServicesDetails 的方法

            var cart = _context.Cart_ExtraServicesDetails
                .Where(c => c.CartId == cartid)
                .Include(c => c.ExtraServiceProduct) // Load the ExtraServiceProduct
                .Include(c => c.ExtraServiceProduct.ExtraService) // Load the ExtraService within ExtraServiceProduct
                .ToList(); ;
            // 使用 View 屬性設定要回傳的檢視名稱
            return View("ExtraServicesDetailsPartialView", cart);
        }
    }
}
