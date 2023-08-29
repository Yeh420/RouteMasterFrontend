using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Infra.EFRepositories;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.Interfaces;
using RouteMasterFrontend.Models.Services;
using RouteMasterFrontend.Models.ViewModels.AttractionVMs;
using RouteMasterFrontend.Models.ViewModels.Members;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace RouteMasterFrontend.Controllers
{
    public class AttractionsController : Controller
    {
        public async Task<IActionResult> Index(AttractionCriteria criteria, int page = 1)
        {
            IEnumerable<AttractionIndexVM> attractions = await GetAttractions();

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
            if (criteria.order == "clickInThirty")
            {
                attractions = attractions
                    .Where(a => a.ClicksInThirty > 0)
                    .OrderByDescending(a => a.ClicksInThirty);
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

        public async Task<IActionResult> Map()
        {
            var repo = new AttractionEFRepository();

            var atts = await repo.GetAtts();

            return View(atts);
        }

        public async Task<IActionResult> Details(int id)
        {
            AddClick(id);
            AttractionDetailVM vm = Get(id);

            
            // 在這裡檢查景點是否已加入最愛，並將結果傳遞到視圖
            bool isFavorite = CheckIfFavorite(id); // 需要實現這個方法來檢查是否已加入最愛
            ViewBag.IsFavorite = isFavorite; // 將結果傳遞到視圖中

            List<string> tags = Get(id).Tags;
            IEnumerable<AttractionIndexVM> attractions = await GetAttractions();

            if (tags != null)
            {
                attractions = attractions.Where(a => a.Tags.Intersect(tags).Any() && a.Id != id)
                    .OrderByDescending(a => a.ClicksInThirty)
                    .Take(6);
            }

            vm.RelatedAttractions = attractions;

            var repo = new AttractionEFRepository();
            IEnumerable<AttractionForDistsnceVM> atts = repo.GetAllWithXY();

            var targetX = atts.Where(a => a.Id == id).Select(a => a.PosX).FirstOrDefault();
            var targetY = atts.Where(a => a.Id == id).Select(a => a.PosY).FirstOrDefault();

            List<AttractionForDistsnceVM> sortedAttractions = atts
                .Where(a => a.Id != id) // Exclude the target attraction itself
                .ToList();

            foreach (var attraction in sortedAttractions)
            {
                if (targetY.HasValue && targetX.HasValue && attraction.PosX.HasValue && attraction.PosY.HasValue)
                {
                    decimal distance = CalculateDistance(targetY.Value, targetX.Value, attraction.PosY.Value, attraction.PosX.Value);
                    attraction.Distance = distance;
                }
                else
                {
                    attraction.Distance = decimal.MaxValue;
                }
            }

            sortedAttractions = sortedAttractions
                .OrderBy(a => a.Distance) // Sort attractions by distance
                .Take(6)
                .ToList();

            vm.CloseAtt = sortedAttractions;


            IEnumerable<AttractionIndexVM> sameRegionAtt = await GetAttractions();
            var region = sameRegionAtt.Where(a => a.Id == id).Select(a => a.Region).FirstOrDefault();

            sameRegionAtt = sameRegionAtt.Where(a => a.Region == region && a.Id != id)
                    .OrderByDescending(a => a.ClicksInThirty)
                    .Take(6);
            

            vm.SameRegionAtt = sameRegionAtt;


            IEnumerable<AttractionIndexVM> sameCategoryAtt = await GetAttractions();
            var category = sameCategoryAtt.Where(a => a.Id == id).Select(a => a.AttractionCategory).FirstOrDefault();

            sameCategoryAtt = sameCategoryAtt.Where(a => a.AttractionCategory == category && a.Id != id)
                    .OrderByDescending(a => a.ClicksInThirty)
                    .Take(6);


            vm.SameCategoryAtt = sameCategoryAtt;

            return View(vm);
        }

        public static decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double earthRadius = 6371.0; // Earth's radius in kilometers

            double lat1Rad = DegreesToRadians((double)lat1);
            double lon1Rad = DegreesToRadians((double)lon1);
            double lat2Rad = DegreesToRadians((double)lat2);
            double lon2Rad = DegreesToRadians((double)lon2);

            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(deltaLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            decimal distance = (decimal)(earthRadius * c); // Distance in kilometers

            return distance;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }


        private bool CheckIfFavorite(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerAccount = User.Identity.Name;
                return GetFavoriteAtt(customerAccount).Select(a=>a.Id).Contains(id);
            }
            else
            {
                return false;
            }
            
        }

        public IActionResult AddToFavorite (int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerAccount = User.Identity.Name;
                Add2Favorite(customerAccount, id);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult RemoveFromFavorite(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerAccount = User.Identity.Name;
                RemoveAttFromFavorite(customerAccount, id);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        private void RemoveAttFromFavorite(string? customerAccount, int id)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            service.RemoveAttFromFavorite(customerAccount, id);
        }

        
        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult FavoriteAtt(int page = 1)
        {
            var customerAccount = User.Identity.Name;
            IEnumerable<AttractionIndexVM> attractions = GetFavoriteAtt(customerAccount);

            int pageSize = 15;

            int totalItems = attractions.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            attractions = attractions.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(attractions);
        }

        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetFavoriteAtt(int page = 1)
        {
            var customerAccount = User.Identity.Name;
            IEnumerable<AttractionIndexVM> attractions = GetFavoriteAtt(customerAccount);

            int pageSize = 15;

            int totalItems = attractions.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            attractions = attractions.Skip((page - 1) * pageSize).Take(pageSize);

            

            return Content(JsonConvert.SerializeObject(attractions), "application/json");
        }

        private IEnumerable<AttractionIndexVM> GetFavoriteAtt(string? customerAccount)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            return service.GetFavoriteAtt(customerAccount).Select(dto => dto.ToIndexVM());
        }

        private void Add2Favorite(string? customerAccount, int id)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            service.AddToFarvorite(customerAccount, id);
        }

        private void AddClick(int id)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            service.AddClick(id);
        }

        private AttractionDetailVM Get(int id)
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            return service.Get(id).ToDetailVM();
        }

        private async Task<IEnumerable<AttractionIndexVM>> GetAttractions()
        {
            IAttractionRepository repo = new AttractionEFRepository();
            AttractionService service = new AttractionService(repo);

            var dtos = service.Search();
            var vms = dtos.Select(dto => dto.ToIndexVM());

            return vms;

        }
    }
}
