using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using System.Security.Claims;

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
            ClaimsPrincipal user = HttpContext.User;
            int userID = int.Parse(user.FindFirst("id").Value);

            var messageDb = _context.SystemMessages
                .Where(m => m.MemberId == userID)
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


            if (!msg.IsRead)
            {
                msg.IsRead = true;
                _context.SystemMessages.Update(msg);
                _context.SaveChanges();

                string result = $"通知編號:{id}已被列為已讀";
                return result;

            }
            else
            {
                msg.IsRead= false;
                _context.SystemMessages.Update(msg);
                _context.SaveChanges();

                return $"通知編號:{id}已回復成未讀狀態";
            }
            
        }

        [HttpPost]
        public async Task<string> MarkAllAsRead()
        {
            var targetMsg= await _context.SystemMessages
                .Where(m=>m.IsRead == false)
                .ToListAsync();
            foreach( var item in targetMsg )
            {
                item.IsRead = true;
            }
            await _context.SaveChangesAsync();

            return "全已讀修改完成";
        }
        [HttpPost]
        public async Task<string> GetUrl(int msgId)
        {
            string content = await _context.SystemMessages
                .Where(m => m.Id == msgId)
                .Select(m => m.Content).FirstAsync();

            string endText = "評論區";
            int endIndex = content.IndexOf(endText);

            string spotName=content.Substring(2, (endIndex-2));
            
            int AttractionId= await _context.Attractions
                .Where(a=>a.Name == spotName)
                .Select(a=>a.Id).FirstAsync();

            var url = $"https://localhost:7145/Attractions/Details/{AttractionId}";

            return url;
        }
    }
}
