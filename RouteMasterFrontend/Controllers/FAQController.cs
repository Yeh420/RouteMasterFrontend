using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;
using System.Runtime.Serialization.Json;

namespace RouteMasterFrontend.Controllers
{
    public class FAQController : Controller
    {
        private readonly RouteMasterContext _context;
        public FAQController(RouteMasterContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Search()
        {
            var questList = _context.FAQs
                .Include(f => f.FAQCategory);


            var dto = await questList.Select(f=>new FAQIndexDTO
            {
                Id = f.Id,
                CategoryName=f.FAQCategory.Name,
                Question=f.Question,
                Answer=f.Answer,
                Helpful=f.Helpful,
                Image=f.Image,
            }).ToListAsync();
                
                
            return Json(dto);
        }
    }
}
