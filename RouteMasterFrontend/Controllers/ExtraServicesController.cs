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

        public ExtraServicesController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: ExtraServices
        public async Task<IActionResult> Index()
        {
            var routeMasterContext = _context.ExtraServices.Include(e => e.Attraction).Include(e => e.Region);
            return View(await routeMasterContext.ToListAsync());
        }

        // GET: ExtraServices/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ExtraServices/Create
        public IActionResult Create()
        {
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View();
        }

        // POST: ExtraServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegionId,AttractionId,Name,Description,Status,Image")] ExtraService extraService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(extraService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", extraService.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", extraService.RegionId);
            return View(extraService);
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

        private bool ExtraServiceExists(int id)
        {
          return (_context.ExtraServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
