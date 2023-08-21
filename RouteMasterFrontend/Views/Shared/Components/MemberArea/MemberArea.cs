using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.ViewModels.Members;
using System.Security.Claims;

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
            ClaimsPrincipal user = HttpContext.User;
            var id = user.FindFirst("id").Value;
            int memberid = int.Parse(id);
            Member myMember = _context.Members.First(m => m.Id == memberid);
            var modelPasword = new MemberEditPasswordVM();
            modelPasword.id =memberid;
            
            switch (pagecase)
            {
                case 0:
                    return View("MemEdit", myMember);
                case 1:
                    return View("MemOrder",memberid);
                case 2:                   
                    return View("EditPassword", modelPasword);
                case 3:
                    return View("_MessagePartial");

            }

            var model = new MemberEditPasswordVM();
            return View("EditPassword", model);
        }

       
    }
}
