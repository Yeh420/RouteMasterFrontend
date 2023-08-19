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
        public async Task<JsonResult> Index(int filter)
        {
            var messageDb = _context.SystemMessages
                .Where(m => m.MemberId == 1)
                .OrderByDescending(m => m.Id)
                .AsQueryable();

            switch (filter)
            {
                case 1:
                    messageDb = messageDb.Where(m => m.IsRead == false);
                    break;
                case 2:
                    messageDb = messageDb.Where(m => m.IsRead == true);
                    break;
                case 3:
                   
                    break;
               
            }
            var dto= await messageDb

                .Select(m=>new SystemMessageAjaxDTO
                {
                    Id = m.Id,
                    Category= m.Content.Contains("檢舉") ? "檢舉" : (m.Content.Contains("按讚") ? "按讚" : "回覆"),
                    Content = m.Content,
                    IsRead = m.IsRead,
                
            
                   
                }).ToListAsync();

            return Json(dto);
        }
        [HttpPost]
        public async Task<string> UpdateNoticeStatus(int id)
        {
            SystemMessage msg= await _context.SystemMessages
                .Where (m=>m.Id == id).FirstAsync();

            msg.IsRead = true;
            _context.SystemMessages.Update(msg);
            _context.SaveChanges();

            string result = $"通知編號:{id}已被列為已讀";

            return result;
            
        }
    }
}
