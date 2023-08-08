using Microsoft.AspNetCore.Mvc;
using RouteMasterFrontend.Models.Infra.EFRepositories;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.Interfaces;
using RouteMasterFrontend.Models.Services;
using RouteMasterFrontend.Models.ViewModels.AttractionVMs;

namespace RouteMasterFrontend.Controllers
{
    public class AttractionsController : Controller
    {
        public IActionResult Index(AttractionCriteria criteria, int page = 1)
        {
            IEnumerable<AttractionIndexVM> attractions = GetAttractions();

            ViewBag.Categories = attractions.Select(a => a.AttractionCategory).Distinct().ToList();
            ViewBag.Tags = attractions.SelectMany(a => a.Tags).Distinct().ToList();
            ViewBag.Regions = attractions.Select(a => a.Region).Distinct().ToList();

            ViewBag.Criteria = criteria;

            #region where
            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                attractions = attractions.Where(a => a.Name.Contains(criteria.Keyword));
            }
            if (criteria.category != null)
            {
                attractions = attractions.Where(a => criteria.category.Contains(a.AttractionCategory));
            }
            if (criteria.tag != null)
            {
                attractions = attractions.Where(a => a.Tags.Intersect(criteria.tag).Any());
            }
            if (criteria.region != null)
            {
                attractions = attractions.Where(a => criteria.region.Contains(a.Region));
            }
            if (criteria.order == "click")
            {
                attractions = attractions.OrderByDescending(a => a.Clicks);
            }
            if (criteria.order == "score")
            {
                attractions = attractions.OrderByDescending(a => a.Score);
            }
            if (criteria.order == "hours")
            {
                attractions = attractions.OrderBy(a => a.Hours);
            }
            if (criteria.order == "hoursDesc")
            {
                attractions = attractions.OrderByDescending(a => a.Hours);
            }
            if (criteria.order == "price")
            {
                attractions = attractions.OrderBy(a => a.Price);
            }
            if (criteria.order == "priceDesc")
            {
                attractions = attractions.OrderByDescending(a => a.Price);
            }
            if (criteria.order == "")
            {
                attractions = attractions.OrderByDescending(a => a.Id);
            }

            #endregion

            ViewBag.Count = attractions.ToList().Count();
            int pageSize = 15;

            int totalItems = attractions.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            attractions = attractions.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(attractions);
        }

        public IActionResult Details(int id)
        {
            AttractionDetailVM vm = Get(id);

            return View(vm);
        }

        private AttractionDetailVM Get(int id)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            return service.Get(id).ToDetailVM();
        }

        private IEnumerable<AttractionIndexVM> GetAttractions()
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            return service.Search().Select(dto => dto.ToIndexVM());
        }
    }
}
