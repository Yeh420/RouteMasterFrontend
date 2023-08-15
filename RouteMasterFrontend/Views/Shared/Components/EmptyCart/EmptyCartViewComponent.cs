using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RouteMasterFrontend.Views.Shared.Components.EmptyCart
{
    public class EmptyCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            return View("Default");
        }
    }
}
