using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Controllers
{
    public class ExtraServicesController : Controller
    {
        private readonly RouteMasterContext _context;
        private readonly IWebHostEnvironment _environment;

        public ExtraServicesController(RouteMasterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ExtraServices
        public async Task<IActionResult> Index()
        {           
            return  View();
        }

        // GET: ExtraServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {         
         
          ViewBag.ExtraServiceId=id;

            return View();
        }

        // GET: ExtraServices/Create
        public IActionResult Create()
        {
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View();
        }

        // POST: ExtraServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExtraService extraService, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    string path = Path.Combine(_environment.WebRootPath, "ExtraServiceImages");
                    string fileName = SaveUploadFile(path, file);
                    extraService.Image = fileName;

                    _context.Add(extraService);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }            
            }
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name", extraService.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", extraService.RegionId);
            return View(extraService);
        }



        public IActionResult UploadExtraServiceImages(int id)
        {
            var extraServiceInDb = _context.ExtraServices.Where(e => e.Id == id).FirstOrDefault();
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Name");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View(extraServiceInDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadExtraServiceImages(ExtraService extraService, IFormFile[] files)
        {
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    string path = Path.Combine(_environment.WebRootPath, "ExtraServiceImages");
                    string fileName = SaveUploadFile(path, file);
                    ExtraServiceImage img = new ExtraServiceImage();
                    img.ExtraServiceId = extraService.Id;
                    img.Image = fileName;
                    _context.ExtraServiceImages.Add(img);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }




        public IActionResult CreateExtraServiceProduct(int Id)
        {
            ExtraServiceProduct product = new ExtraServiceProduct();
            product.ExtraServiceId = Id;
            ViewBag.CurrentDate = DateTime.Today;
            ViewData["ExtraServiceId"] = new SelectList(_context.ExtraServices, "Id", "Name");
            return View(product);
        }




        [HttpPost]
        public IActionResult CreateExtraServiceProduct(ExtraServiceProduct extraServiceProduct, int interValDays)
        {
            //ctrl+shift+h全文件搜尋
            //一次新增多筆產品資料
            for (int i = 0; i < interValDays; i++)
            {
                ExtraServiceProduct product = new ExtraServiceProduct();
                product.Date = extraServiceProduct.Date.AddDays(i).Date;
                product.ExtraServiceId = extraServiceProduct.ExtraServiceId;     
                product.Quantity = extraServiceProduct.Quantity;
                product.Price = extraServiceProduct.Price;
                _context.ExtraServiceProducts.Add(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }




        public IActionResult ExtraServiceProductsIndex()
        {
            return View(_context.ExtraServiceProducts);
        }





















        // GET: ExtraServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExtraServices == null)
            {
                return NotFound();
            }

            var extraService = await _context.ExtraServices.FindAsync(id);
            if (extraService == null)
            {
                return NotFound();
            }
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", extraService.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", extraService.RegionId);
            return View(extraService);
        }

        // POST: ExtraServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegionId,AttractionId,Name,Description,Status,Image")] ExtraService extraService)
        {
            if (id != extraService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extraService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtraServiceExists(extraService.Id))
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
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", extraService.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", extraService.RegionId);
            return View(extraService);
        }

        // GET: ExtraServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExtraServices == null)
            {
                return NotFound();
            }

            var extraService = await _context.ExtraServices
                .Include(e => e.Attraction)
                .Include(e => e.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extraService == null)
            {
                return NotFound();
            }

            return View(extraService);
        }

        // POST: ExtraServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExtraServices == null)
            {
                return Problem("Entity set 'RouteMasterContext.ExtraServices'  is null.");
            }
            var extraService = await _context.ExtraServices.FindAsync(id);
            if (extraService != null)
            {
                _context.ExtraServices.Remove(extraService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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


        private bool ExtraServiceExists(int id)
        {
          return (_context.ExtraServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
