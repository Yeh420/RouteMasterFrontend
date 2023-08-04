using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IViewComponentResult Invoke()
        {

            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");

            // 將讀取的值存入 ViewData
            ViewData["CartId"] = cartIdFromCookie;

            return View("CartPartial");
        }


    }
}
