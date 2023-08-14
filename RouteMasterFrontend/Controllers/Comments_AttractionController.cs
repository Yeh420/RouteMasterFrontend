using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;

namespace RouteMasterFrontend.Controllers
{
    public class Comments_AttractionController : Controller
    {
        private readonly RouteMasterContext _context;
        private readonly IWebHostEnvironment _environment;

        public Comments_AttractionController(RouteMasterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Comments_AttractionIndexPartial()
        {
            return View();
        }   
        public async Task<JsonResult> GetComments([FromBody] Comments_AttractionAjaxDTO input)
        {
            var commentDb = _context.Comments_Attractions
                .Include(c => c.Member)
                .Include(c => c.Attraction)
                .Where(c => c.AttractionId == input.SpotId);
                
            switch (input.Manner)
            {
                case 0:
                    commentDb = commentDb.OrderBy(c => c.Id);
                    break;
                case 1:
                    commentDb = commentDb.OrderByDescending(c => c.CreateDate);
                    break;
                case 2:
                    commentDb = commentDb.OrderByDescending(c => c.Score);
                    break;
                case 3:
                    commentDb = commentDb.OrderBy(c => c.Score);
                    break;
                case 4:
                    commentDb = commentDb.OrderByDescending(c => c.StayHours);
                    break;
                case 5:
                    commentDb = commentDb.OrderByDescending(c => c.Price);
                    break;
            }

            var proImg = _context.Comments_AttractionImages;


            var records= await commentDb.Select(c=> new Comments_AttractionIndexDTO
            {
                Id = c.Id,
                Account=c.Member.Account,
                AttractionName= c.Attraction.Name,
                Score= c.Score,
                Content= c.Content,
                StayHours= (int)c.StayHours,
                Price= (int)c.Price,
                CreateDate= c.CreateDate,
                IsHidden= c.IsHidden,
                ImageList=proImg.Where(p=>p.Comments_AttractionId==c.Id)
                .Select(p=>p.Image).ToList(),

            }).ToListAsync();

            return Json(records);
        }
    }
}
