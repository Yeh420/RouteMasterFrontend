using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Carts.Components.AccomodationDetails
{
    public class AccomodationDetailsViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _routeMasterContext;
        public AccomodationDetailsViewComponent(RouteMasterContext routeMasterContext)
        {
            _routeMasterContext = routeMasterContext;
        }
        public IViewComponentResult Invoke(int cartid)
        {
            var cart = _routeMasterContext.Cart_AccommodationDetails
            .Where(c => c.CartId == cartid)
            .Include(c => c.RoomProduct)
            .Include(c => c.RoomProduct.Room)
            .Include(c => c.RoomProduct.Room.Accommodation)
            .ToList();
            if (cart.Any())
            {
                return View("AccommodationDetailsPartialView", cart);
            }
            else
            {
                return Content(""); // 返回一个空的内容
            }
        }
    
    }
    
}
