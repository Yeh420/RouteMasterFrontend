using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;
using static Dapper.SqlMapper;

namespace RouteMasterFrontend.Controllers
{
    public class Comments_AccommodationController : Controller
    {
        private readonly RouteMasterContext _context;
        private readonly IWebHostEnvironment _environment;
        public Comments_AccommodationController(RouteMasterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Comments_Accommodation
        [HttpGet]
        public async Task<JsonResult> Index(int stayId)
        {
            var proLike = _context.Comment_Accommodation_Likes;

            var commentDb = await _context.Comments_Accommodations
                  .Include(c => c.Member)
                  .Include(c => c.Accommodation)
                  .Where(c => c.AccommodationId == stayId)
                  .AsNoTracking()
                  .OrderByDescending(c=>c.CreateDate)
                  .Select(c => new Comments_AccommodationBriefDTO
                  {
                      Id = c.Id,
                      Account = c.Member.Account,
                      Score = c.Score,
                      Title = c.Title.Length > 10 ? c.Title.Substring(0, 10) + "..." : c.Title,
                      Pros = c.Pros,
                      Cons = c.Cons,
                      CreateDate = TransCommentCreate(c.CreateDate),
                      TotalThumbs = proLike.Where(l => l.Comments_AccommodationId == c.Id).Count(),
                  }).Take(3).ToListAsync();
      

            return Json(commentDb);
		}

        public async Task<JsonResult> ImgSearch([FromBody] Comments_AccommodationAjaxDTO input)
        {
           
            var commentDb = _context.Comments_Accommodations
                  .Include(c => c.Member)
                  .Include(c => c.Accommodation)
                  .Include(c => c.CommentStatus)

                  .Where(c => c.AccommodationId == input.HotelId);
          
            switch (input.Manner)
            {
                case 0:
					commentDb = commentDb.OrderByDescending(c => c.CreateDate);
					break;
                case 1:
					commentDb = commentDb.OrderByDescending(c => c.Score);
					break;
                case 2:
					commentDb = commentDb.OrderBy(c => c.Score);
					break;
            }

            var proImg = _context.Comments_AccommodationImages;
            var proLike = _context.Comment_Accommodation_Likes
                .Include(l => l.Member);

            
            var rod = await commentDb.Select(c => new Comments_AccommodationIndexDTO
            {
                Id = c.Id,
                Account = c.Member.Account,
                HotelName = c.Accommodation.Name,
                Score = c.Score,
                Title = c.Title,
                Pros = c.Pros,
                Cons = c.Cons,
                CreateDate = TransCommentCreate(c.CreateDate),
                Status = c.CommentStatus.Name,
                ReplyMessage = c.Reply,
                ReplyDate = c.ReplyAt.HasValue ? c.ReplyAt.Value.ToString("yyyy/MM/dd") : null,
                ImageList = proImg.Where(p => p.Comments_AccommodationId == c.Id)
                .Select(p => p.Image).ToList(),
                ThumbsUp = proLike.Any(l => l.Comments_AccommodationId == c.Id && l.MemberId == 1),
                TotalThumbs = proLike.Where(l => l.Comments_AccommodationId == c.Id).Count(),
                Profile= c.Member.Image,
                Gender= c.Member.Gender,
                Address= c.Member.Address.Length >= 3 ? c.Member.Address.Substring(0, 3) : c.Member.Address,

            }).ToListAsync();

            return Json(rod);
        }

        private static string TransCommentCreate(DateTime createDate)
        {
            TimeSpan sinceCreation = DateTime.Now - createDate;
            try
            {
                if (sinceCreation.TotalDays > 365)
                {
                    int years = (int)(sinceCreation.TotalDays / 365);
                    return $"{years} 年前";
                }
                else if (sinceCreation.TotalDays > 60)
                {
                    return "多個月前";
                }
                else if(sinceCreation.TotalDays > 3)
                {
                    return "多天前";
                }
                else if (sinceCreation.TotalHours >= 24)
                {
                    return $"{(int)sinceCreation.TotalDays} 天前";
                }
                else if (sinceCreation.TotalHours >= 1)
                {
                    return $"{(int)sinceCreation.TotalHours} 小時前";
                }
                else if (sinceCreation.TotalMinutes >= 1)
                {
                    return $"{(int)sinceCreation.TotalMinutes} 分鐘前";
                }               
                else
                {
                    return "剛剛";
                }

            }
            catch (Exception ex)
            {
                return $"錯誤: {ex.Message}";
            }
            
        }

        public IActionResult PartialPage()  
        {
            return View();
        }


        public async Task<bool> DecideLike ([FromBody] Comments_LikesAjaxDTO input)
        {
            ClaimsPrincipal user = HttpContext.User;
            string userAccount = user.Identity.Name;
            int userID = int.Parse(user.FindFirst("id").Value);

            var proLike =await _context.Comment_Accommodation_Likes
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Comments_AccommodationId == input.CommentId && l.MemberId == userID);
            // @@l.MemberAccount==user.Identity.name

            var info = _context.Comments_Accommodations
                   .Include(c => c.Member)
                   .Where(c => c.Id == input.CommentId)
                   .Select(c => c.ToFilterDto());

            int id = info.Select(c => c.MemberId).First();
            var title = info.Select(c => c.CommentTitle).First();

            if (proLike==null)
            {
                //建立按讚紀錄
                Comment_Accommodation_Like like = new Comment_Accommodation_Like
                {
                    MemberId = userID, //@@記得改user.Identity.name
                    Comments_AccommodationId = input.CommentId,
                    CreateDate = DateTime.Now,
                    
                };   
                _context.Comment_Accommodation_Likes.Add(like);
                await _context.SaveChangesAsync();

               


                //按讚通知生成
                SystemMessage msg = new SystemMessage
                {
                    //@@按讚人記得改user.Identity.name
                    MemberId =id,
                    Content =$"{userAccount}對您標題為:{title}的評論按讚",
                    IsRead = false,
                };
                _context.SystemMessages.Add(msg);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                //刪除按讚紀錄
                _context.Comment_Accommodation_Likes.Remove(proLike);
                await _context.SaveChangesAsync();

                //刪除通知
                var matchContent = $"{userAccount}對您標題為:{title}的評論按讚"; //記得按讚人改user.Identity.name

                SystemMessage matchMsg= await _context.SystemMessages
                    .Where(m=>m.MemberId==id && m.Content==matchContent)
                    .FirstAsync();

                if (matchMsg!=null)
                {
                    _context.SystemMessages.Remove(matchMsg);
                    await _context.SaveChangesAsync();
                }
            

                return false;
            }
            

        }
        [HttpPost]
        public async Task<string> NewComment([FromForm] Comments_AccommodationCreateDTO dto)
        {
            ClaimsPrincipal user = HttpContext.User;
            int userID = int.Parse(user.FindFirst("id").Value);

            Comments_Accommodation commentDb = new Comments_Accommodation
            {
                MemberId = userID, //user,identity.id
                AccommodationId = dto.AccommodationId,               
                Score = dto.Score,
                Title = dto.Title,
                Pros = dto.Pros,
                Cons = dto.Cons,
                CommentStatusId = 1, //預設訊息未回覆狀態
                CreateDate = DateTime.Now,
            };
            _context.Comments_Accommodations.Add(commentDb);
            _context.SaveChanges();

            string webRootPath = _environment.WebRootPath;
            string path = Path.Combine(webRootPath, "MemberUploads");

            if(dto.Files != null)
            {
                foreach(IFormFile i in dto.Files)
                {
                    if(i != null && i.Length > 0)
                    {
                        Comments_AccommodationImage img = new Comments_AccommodationImage();
                        string fileName = SaveUploadedFile(path, i);

                        img.Comments_AccommodationId = commentDb.Id;
                        img.Image = fileName;
                        _context.Comments_AccommodationImages.Add(img);
                    }
                }
                _context.SaveChanges();
                return "新增含圖評論成功";
            }
            return "新增無圖評論成功";
        }

        // GET: Comments_Accommodation/Details/5    
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments_Accommodations == null)
            {
                return NotFound();
            }

            var comments_Accommodation = await _context.Comments_Accommodations
                .Include(c => c.Accommodation)
                .Include(c => c.CommentStatus)
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comments_Accommodation == null)
            {
                return NotFound();
            }

            return View(comments_Accommodation);
        }

        // GET: Comments_Accommodation/Create
        public IActionResult Create()
        {
            ViewBag.AccommodationId = new SelectList(_context.Accommodations, "Id", "Name");
            //ViewData["CommentStatusId"] = new SelectList(_context.CommentStatuses, "Id", "Name");
            ViewBag.MemberId = new SelectList(_context.Members, "Id", "Account");
            return View();
        }

        // POST: Comments_Accommodation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comments_AccommodationCreateVM vm, List<IFormFile> file1) 
        {
            ViewBag.AccommodationId = new SelectList(_context.Accommodations, "Id", "Name", vm.AccommodationId);
            //ViewData["CommentStatusId"] = new SelectList(_context.CommentStatuses, "Id", "Name", vm.CommentStatusId);
            ViewBag.MemberId = new SelectList(_context.Members, "Id", "Account", vm.MemberId);
            if (ModelState.IsValid)
            {
                Comments_Accommodation commentDb = new Comments_Accommodation
                {
                    AccommodationId = 1,//vm.AccommodationId,
                    MemberId = 2,
                    Score = vm.Score,
                    Title = vm.Title,
                    Pros = vm.Pros,
                    Cons = vm.Cons,
                    CommentStatusId = 1, //預設訊息未回覆狀態
                    CreateDate = DateTime.Now,
                };

                _context.Comments_Accommodations.Add(commentDb);
                _context.SaveChanges();
                string webRootPath = _environment.WebRootPath;

                string path = Path.Combine(webRootPath, "MemberUploads");               

                foreach (IFormFile i in file1)
                {
                    if (i != null && i.Length > 0)
                    {
                        Comments_AccommodationImage img = new Comments_AccommodationImage();
                        string fileName = SaveUploadedFile(path, i);
                   
                        img.Comments_AccommodationId = commentDb.Id;
                        img.Image = fileName;
                        _context.Comments_AccommodationImages.Add(img);                    
                    }
                }

                _context.SaveChanges();



                return RedirectToAction("Index","Home");

            }
            //ModelState.AddModelError("", "請點擊星星給予評分");
            return View(vm);
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

        // GET: Comments_Accommodation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments_Accommodations == null)
            {
                return NotFound();
            }

            var comments_Accommodation = await _context.Comments_Accommodations.FindAsync(id);
            if (comments_Accommodation == null)
            {
                return NotFound();
            }
            ViewData["AccommodationId"] = new SelectList(_context.Accommodations, "Id", "Address", comments_Accommodation.AccommodationId);
            ViewData["CommentStatusId"] = new SelectList(_context.CommentStatuses, "Id", "Name", comments_Accommodation.CommentStatusId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", comments_Accommodation.MemberId);
            return View(comments_Accommodation);
        }

        // POST: Comments_Accommodation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,AccommodationId,Score,Title,Pros,Cons,CreateDate,CommentStatusId,Reply,ReplyAt")] Comments_Accommodation comments_Accommodation)
        {
            if (id != comments_Accommodation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comments_Accommodation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Comments_AccommodationExists(comments_Accommodation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccommodationId"] = new SelectList(_context.Accommodations, "Id", "Address", comments_Accommodation.AccommodationId);
            ViewData["CommentStatusId"] = new SelectList(_context.CommentStatuses, "Id", "Name", comments_Accommodation.CommentStatusId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", comments_Accommodation.MemberId);
            return View(comments_Accommodation);
        }

        // GET: Comments_Accommodation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments_Accommodations == null)
            {
                return NotFound();
            }

            var comments_Accommodation = await _context.Comments_Accommodations
                .Include(c => c.Accommodation)
                .Include(c => c.CommentStatus)
                .Include(c => c.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comments_Accommodation == null)
            {
                return NotFound();
            }

            return View(comments_Accommodation);
        }

        // POST: Comments_Accommodation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments_Accommodations == null)
            {
                return Problem("Entity set 'RouteMasterContext.Comments_Accommodations'  is null.");
            }
            var comments_Accommodation = await _context.Comments_Accommodations.FindAsync(id);
            if (comments_Accommodation != null)
            {
                _context.Comments_Accommodations.Remove(comments_Accommodation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Comments_AccommodationExists(int id)
        {
          return (_context.Comments_Accommodations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
