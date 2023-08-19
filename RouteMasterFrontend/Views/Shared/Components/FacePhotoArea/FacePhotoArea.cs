using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.ViewModels.Members;

namespace RouteMasterFrontend.Views.Shared.Components.FacePhoto
{
    public class FacePhotoAreaViewComponent : ViewComponent
    {
        private readonly RouteMasterContext _context;
        public FacePhotoAreaViewComponent(RouteMasterContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View("FacePhoto");
        }

    }
}
