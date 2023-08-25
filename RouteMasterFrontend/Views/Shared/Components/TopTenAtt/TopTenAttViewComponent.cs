using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.Models.Infra.EFRepositories;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.Interfaces;
using RouteMasterFrontend.Models.Services;
using RouteMasterFrontend.Models.ViewModels.AttractionVMs;

namespace RouteMasterFrontend.Views.Shared.Components.TopTenAtt
{
    public class TopTenAttViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            IEnumerable<AttractionTopTenVM> topTenAtt = GetTopTenAtt();

            return View("TopTenAttPartialView", topTenAtt);
        }

        private IEnumerable<AttractionTopTenVM> GetTopTenAtt()
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            return service.GetTopTen().Select(dto => dto.ToTopTenVM());
        }
    }
}
