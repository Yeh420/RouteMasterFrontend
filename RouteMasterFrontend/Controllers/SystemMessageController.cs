using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;

namespace RouteMasterFrontend.Controllers
{
    public class SystemMessageController : Controller
    {
        private readonly RouteMasterContext _context;

        public SystemMessageController(RouteMasterContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<JsonResult> Index()
        {
            var messageDb= await _context.SystemMessages
                .Where(m=>m.MemberId ==1)
                .Select(m=>new SystemMessageAjaxDTO
                {
                    Id = m.Id,
                    Category= m.Content.Contains("檢舉") ? "檢舉" : (m.Content.Contains("按讚") ? "按讚" : "回覆"),
                    Content = m.Content,
                    IsRead = m.IsRead,
                
            
                   
                }).ToListAsync();
                

            return Json(messageDb);
        }
    }
}
