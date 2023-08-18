using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Infra.Criterias;
using RouteMasterFrontend.Models.Infra.DapperRepositories;
using RouteMasterFrontend.Models.Infra.EFRepositories;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.Interfaces;
using RouteMasterFrontend.Models.Services;

namespace RouteMasterFrontend.Controllers
{
    public class ActivitiesController : Controller
    {
		private readonly IWebHostEnvironment _environment;
		private readonly RouteMasterContext _context;
  

        public ActivitiesController(RouteMasterContext context, IWebHostEnvironment environment)
        {
            _context = context;
			_environment = environment;
		}

        // GET: ActivitiesE
        public IActionResult Index()
        {

            return View();
        }















       
        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.ActivityId = id;

            return View();
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name");
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Activity activity,IFormFile file)
        {
			if (ModelState.IsValid)
			{

				if (file != null && file.Length > 0)
				{
					string path = Path.Combine(_environment.WebRootPath, "ActivityImages");
					string fileName = SaveUploadFile(path, file);
					activity.Image = fileName;
					_context.Add(activity);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
                }
			}

			ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name", activity.ActivityCategoryId);
			ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name", activity.AttractionId);
			ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", activity.RegionId);
			return View(activity);
		}



        public IActionResult UploadActivityImages(int id)
        {


            var activityInDb = _context.Activities.Where(a => a.Id == id).FirstOrDefault();

            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name");
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");

            return View(activityInDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadActivityImages(Activity activity, IFormFile[] files)
        {


            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    string path = Path.Combine(_environment.WebRootPath, "ActivityImages");
                    string fileName = SaveUploadFile(path, file);
                    ActivityImage img = new ActivityImage();
                    img.ActivityId = activity.Id;
                    img.Image = fileName;
                    _context.ActivityImages.Add(img);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }






        public IActionResult CreateActivityProduct(int Id)
        {
            ActivityProduct product = new ActivityProduct();
            product.ActivityId = Id;
            ViewBag.CurrentDate = DateTime.Today;
            ViewData["ActivityId"] = new SelectList(_context.Activities, "Id", "Name");
            return View(product);
        }




        [HttpPost]
        public IActionResult CreateActivityProduct(ActivityProduct activityProduct, int interValDays)
        {
            //ctrl+shift+h全文件搜尋
            //一次新增多筆產品資料
            for (int i = 0; i < interValDays; i++)
            {
                ActivityProduct product = new ActivityProduct();
                product.Date = activityProduct.Date.AddDays(i).Date;
                product.ActivityId = activityProduct.ActivityId;
                product.StartTime = activityProduct.StartTime;
                product.EndTime = activityProduct.EndTime;
                product.Quantity = activityProduct.Quantity;
                product.Price = activityProduct.Price;
                _context.ActivityProducts.Add(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }






        public IActionResult ActivityProductIndex()
        {

            
            return View(_context.ActivityProducts); 
        }








        private string SaveUploadFile(string filePath, IFormFile file)
		{
			if (file == null || file.Length == 0) return string.Empty;

			string ext = Path.GetExtension(file.FileName);

			string[] allowExts = new string[] { ".jpg", ".jpeg", ".png", ".tif" };

			if (allowExts.Contains(ext.ToLower()) == false)
			{
				return string.Empty;
			}
			string newFileName = Guid.NewGuid().ToString("N") + ext;
			string fullName = Path.Combine(filePath, newFileName);
			using (var fileStream = new FileStream(fullName, FileMode.Create))
			{
				file.CopyTo(fileStream);
			}
			return newFileName;
		}

		// GET: Activities/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name", activity.ActivityCategoryId);
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", activity.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", activity.RegionId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityCategoryId,Name,RegionId,AttractionId,Description,Status,Image")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name", activity.ActivityCategoryId);
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", activity.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", activity.RegionId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityCategory)
                .Include(a => a.Attraction)
                .Include(a => a.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activities == null)
            {
                return Problem("Entity set 'RouteMasterContext.Activities'  is null.");
            }
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
