using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
        public IActionResult IndexSPA()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Search([FromBody] FAQAjaxDTO input)
        {
            var questList = _context.FAQs
                .Include(f => f.FAQCategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(input.Name))
            {
                questList= questList.Where(f => f.FAQCategory.Name == input.Name);
            }
            else
            {
                questList = questList
                   .OrderByDescending(f => f.Helpful)
                   .Take(5);
            }


            if (!string.IsNullOrEmpty(input.Keyword))
            {
                questList = questList.Where(f => f.Question.Contains(input.Keyword));   
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
        [HttpPost]
        public async Task<string> UpdateHelpful(int faqId)
        {
            FAQ quest= await _context.FAQs
                .Include(f=> f.FAQCategory)
                .FirstAsync(f=> f.Id == faqId);
            if(quest != null)
            {
                quest.Helpful = quest.Helpful + 1;
                string result = quest.FAQCategory.Name;

                _context.Update(quest);
                _context.SaveChanges();
               
                return result;
            }

            return "Not Found";
        }
    }
}
