using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Shared.Components.CartPartial
{
    public class CartPartialViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _routeMasterContext;


        public CartPartialViewComponent(RouteMasterContext routeMasterContext)
        {
            _routeMasterContext = routeMasterContext;   
        }

        public IViewComponentResult Invoke(int cartid)
        {
           
            return View("CartPartial");
        }


    }
}
