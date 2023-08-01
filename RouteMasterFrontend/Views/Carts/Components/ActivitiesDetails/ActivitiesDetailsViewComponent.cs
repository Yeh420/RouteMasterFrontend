using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Carts.Components.ActivitiesDetails
{
    public class ActivitiesDetailsViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _routeMasterContext;
        public ActivitiesDetailsViewComponent(RouteMasterContext routeMasterContext)
        {
            _routeMasterContext = routeMasterContext;
        }
        public IViewComponentResult Invoke(int cartid)
        {
            var cart = _routeMasterContext.Cart_ActivitiesDetails
                .Where(c=>c.CartId == cartid)
                .Include(c=>c.ActivityProduct)
                .Include (c=>c.ActivityProduct.Activity)
                .ToList();
            return View("ActivitiesDetailsPartialView",cart);
        }
    }
}
