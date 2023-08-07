using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Views.Shared.Components.SelectAttraction
{
    public class SelectAttractionViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _context;

        public SelectAttractionViewComponent(RouteMasterContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var attractions = _context.Attractions.Include(a => a.Region).Include(a => a.Town).Include(a => a.AttractionCategory);
            return View("_AttractionPartial", attractions);
        }
    }
}
