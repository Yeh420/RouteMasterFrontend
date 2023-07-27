using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Infra.EFRepositories;
using RouteMasterFrontend.Models.Interfaces;
using RouteMasterFrontend.Models.Services;

namespace RouteMasterFrontend.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly RouteMasterContext _context;

        public ActivitiesController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            IActivityRepository repo = new ActivitesListEFRepository();
			ActivityService service = new ActivityService(repo);
			//todo????

			var routeMasterContext = _context.Activities.Include(a => a.ActivityCategory).Include(a => a.Attraction).Include(a => a.Region);
            return View(await routeMasterContext.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name");
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address");
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityCategoryId,Name,RegionId,AttractionId,Description,Status,Image")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityCategoryId"] = new SelectList(_context.ActivityCategories, "Id", "Name", activity.ActivityCategoryId);
            ViewData["AttractionId"] = new SelectList(_context.Attractions, "Id", "Address", activity.AttractionId);
            ViewData["RegionId"] = new SelectList(_context.Regions, "Id", "Name", activity.RegionId);
            return View(activity);
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
