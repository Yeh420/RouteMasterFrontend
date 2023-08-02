using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.ViewModels.Comments_Accommodations;

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
        public async Task<ActionResult<IEnumerable<Comments_AccommodationIndexVM>>> Index([FromBody] Comments_AccommodationAjaxDTO input)
        {
            var commentDb = _context.Comments_Accommodations
                  .Include(c => c.Member)
                  .Include(c => c.Accommodation)
                  .Where(c => c.AccommodationId == input.HotelId);
                 

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
			}

            var vm = await commentDb.AsNoTracking().Select(c => c.ToIndexVM()).ToListAsync();

			return Json(vm);
		}

        public IActionResult PartialPage()
        {
            return View();
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
                    MemberId = vm.MemberId,
                    Score = vm.Score,
                    Title = vm.Title,
                    Pros = vm.Pros,
                    Cons = vm.Cons,
                    CommentStatusId = 1, //預設訊息未回覆狀態
                    CreateDate = DateTime.Now,
                };

                _context.Comments_Accommodations.Add(commentDb);
                await _context.SaveChangesAsync();

                string webRootPath = _environment.WebRootPath;
                string path = Path.Combine(webRootPath, "CommentAccomodationUploads");

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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "請點擊星星給予評分");
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
