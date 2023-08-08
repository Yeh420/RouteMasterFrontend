using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Shared.Components.MemberPartial
{
    public class MemberAreaViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _context;
        public MemberAreaViewComponent(RouteMasterContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View("_MemberPartial");
        }
    }
}
