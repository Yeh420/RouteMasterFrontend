using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Carts.Components.ActivitiesDetails
{
    public class ActivitiesDetailsViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _context;
        public ActivitiesDetailsViewComponent(RouteMasterContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IViewComponentResult Invoke(int cartid)
        {
            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["cartId"] ?? "0");
            ViewData["CartId"]=cartIdFromCookie;

            var cart = _context.Cart_ActivitiesDetails
                .Where(c=>c.CartId == cartid)
                .Include(c=>c.ActivityProduct)
                .Include (c=>c.ActivityProduct.Activity)
                .ToList();
           
             //return View("ActivitiesDetailsPartialView", cart);
            if (cart.Any())
            {
                return View("ActivitiesDetailsPartialView", cart);
            }
            else
            {
                return Content(""); 
            }
        }
    }
}
