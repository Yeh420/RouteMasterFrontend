﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;
using RouteMasterFrontend.Models.ViewModels.Comments_Attractions;

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
                CreateDate= (c.CreateDate).ToString("yyyy/MM/dd"),
                IsHidden = c.IsHidden,
                ImageList=proImg.Where(p=>p.Comments_AttractionId==c.Id)
                .Select(p=>p.Image).ToList(),

            }).ToListAsync();

            return Json(records);
        }

        [HttpPost]
        public async Task<string> NewComment([FromForm] Comments_AttractionCreateDTO dto )
        {
            Comments_Attraction commentDb = new Comments_Attraction
            {
                MemberId = 1,
                AttractionId = dto.AttractionId,
                Score = dto.Score,
                StayHours = dto.StayHours,
                Price = dto.Price,
                Content = dto.Content,
                CreateDate = DateTime.Now,
                IsHidden = false,

            };
            _context.Comments_Attractions.Add(commentDb);
            _context.SaveChanges();

            string webRootPath = _environment.WebRootPath;
            string path = Path.Combine(webRootPath, "MemberUploads");

            if (dto.Files != null)
            {
                foreach (IFormFile i in dto.Files)
                {
                    if (i != null && i.Length > 0)
                    {
                        Comments_AttractionImage img = new Comments_AttractionImage();
                        string fileName = SaveUploadedFile(path, i);

                        img.Comments_AttractionId = commentDb.Id;
                        img.Image = fileName;
                        _context.Comments_AttractionImages.Add(img);
                    }
                }
                _context.SaveChanges();
                return "新增含圖評論成功";
            }
            return "新增無圖評論成功";
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, Comments_AttractionCreateVM vm, List<IFormFile> file1)
        {
            ViewBag.spotId=id;
            if(ModelState.IsValid)
            {
                Comments_Attraction commentDb = new Comments_Attraction
                {
                    AttractionId = id,
                    MemberId = 1, //記得改呼叫user.identity.name
                    Score = vm.Score,
                    Content = vm.Content,
                    StayHours = vm.StayHours,
                    Price = vm.Price,
                    CreateDate = DateTime.Now,
                    IsHidden = false,
                };

                _context.Comments_Attractions.Add(commentDb);
                _context.SaveChanges();

                string webRootPath = _environment.WebRootPath;

                string path = Path.Combine(webRootPath, "MemberUploads");

                foreach (IFormFile i in file1)
                {
                    if (i != null && i.Length > 0)
                    {
                        Comments_AttractionImage img = new Comments_AttractionImage();
                        string fileName = SaveUploadedFile(path, i);

                        img.Comments_AttractionId = commentDb.Id;
                        img.Image = fileName;
                      _context.Comments_AttractionImages.Add(img);
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("Details", "Attractions", new { id = id });
            }
            ModelState.AddModelError("", "請點擊星星給予評分");
            return View(vm);

        }

      
        public async Task<string> ReportComment(int targetId, int reasonId)
        {
            ReportedAttractionComment report = new ReportedAttractionComment
            {
                CommentAttractionId = targetId,
                ReportReasonId = reasonId,
                IsHandled = true,
            };
            _context.ReportedAttractionComments.Add(report);
            _context.SaveChanges();

            var targetComment = _context.ReportedAttractionComments
                .Any(r => r.CommentAttractionId == targetId && r.IsHandled == true);

            var commentDb = _context.Comments_Attractions
                .Include(c=>c.Attraction)
                .Where(c => c.Id == targetId);
               
            int reviewerId= await commentDb.Select(c=>c.MemberId).FirstOrDefaultAsync();
            string spot = await commentDb.Select(c=>c.Attraction.Name).FirstAsync();

            if (targetComment)
            {
                //SystemMessage reportNotice = new SystemMessage
                //{
                //    MemberId = 1, //記得改成user.Identity.name
                //    Content=$"已收到您在{spot}評論區對某評論的檢舉，RouteMaster團隊將依您選擇的檢舉原因審核該評論，再決定是否下架該評論。",
                //    IsRead=false,
                //};

                SystemMessage reportSus = new SystemMessage
                {
                    MemberId = reviewerId,
                    Content = $"您在{spot}評論區的評論，因涉及不良內容，已遭檢舉，審核檢舉成功後，該評論將遭下架。",
                    IsRead = false,

                };
                //_context.SystemMessages.AddRange(reportNotice, reportSus);
                _context.SystemMessages.Add(reportSus);
                _context.SaveChanges() ;

                return "檢舉通知已寄出";
            }
            return "檢舉失敗";
        }

        private string SaveUploadedFile(string path, IFormFile file1)
        {
            // 如果沒有上傳檔案或檔案是空的,就不處理, 傳回 string.empty
            if (file1 == null) return string.Empty;

            // 取得上傳檔案的副檔名
            string ext = System.IO.Path.GetExtension(file1.FileName); // ".jpg" 而不是 "jpg"

            // 如果副檔名不在允許的範圍裡,表示上傳不合理的檔案類型, 就不處理, 傳回 string.empty
            string[] allowedExts = new string[] { ".jpg", ".jpeg", ".png", ".tif" };
            if (allowedExts.Contains(ext.ToLower()) == false) return string.Empty;

            // 生成一個不會重複的檔名
            string newFileName = Guid.NewGuid().ToString("N") + ext; // 生成 亂碼.jpg
            string fullName = System.IO.Path.Combine(path, newFileName);

            // 將上傳檔案存放到指定位置
            using (var stream = new FileStream(fullName, FileMode.Create))
            {
                file1.CopyTo(stream);
            }

            // 傳回存放的檔名
            return newFileName;
        }
    }
}
