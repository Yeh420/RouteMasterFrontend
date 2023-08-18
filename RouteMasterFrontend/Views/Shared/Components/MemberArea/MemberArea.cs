using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.ViewModels.Members;

namespace RouteMasterFrontend.Views.Shared.Components.MemberPartial
{
    public class MemberAreaViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _context;
        public MemberAreaViewComponent(RouteMasterContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int pagecase=0)
        {
            

            switch (pagecase)
            {
                case 0:
                    var modelEdit = new Member();
                    return View("MemEdit", modelEdit);
                case 1:
                    return View("MemOrder");
                case 2:
                    var modelVM = new MemberEditPasswordVM();
                    return View("EditPassword", modelVM);       
            }

            var model = new MemberEditPasswordVM();
            return View("EditPassword", model);
        }

    }
}
