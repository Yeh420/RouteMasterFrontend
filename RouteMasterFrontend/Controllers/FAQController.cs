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
        public async Task<JsonResult> Search([FromBody] FAQAjaxDTO input)
        {
            var questList = _context.FAQs
                .Include(f => f.FAQCategory)
                .Where(f => f.FAQCategory.Name == input.Name);
            string cateName = input.Name;
            var keySearch = input.Keyword;

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                questList = questList.Where(f => f.Question.Contains(input.Keyword) ||
                f.Answer.Contains(input.Keyword));
            }


            var dto = await questList.Select(f => new FAQIndexDTO
            {
                Id = f.Id,
                CategoryName = f.FAQCategory.Name,
                Question = f.Question,
                Answer = f.Answer,
                Helpful = f.Helpful,
                Image = f.Image,
            }).ToListAsync();


            return Json(dto);
        }
    }
}
