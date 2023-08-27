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
    public class TravelPlansController : Controller
    {
        private readonly RouteMasterContext _context;

        public TravelPlansController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: TravelPlans
        public async Task<IActionResult> Index()
        {
            var routeMasterContext = _context.TravelPlans.Include(t => t.Member);
            return View(await routeMasterContext.ToListAsync());
        }

        // GET: TravelPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.PackageTourId = id;
            return View();
        }

        // GET: TravelPlans/Create
        public IActionResult Create()
        {
         
            return View();
        }



        public IActionResult CreateTravelVuePage()
        {
       
            return View();
        }



        public IActionResult PackageTourList()
        {
            return View();
        }



        // POST: TravelPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,TravelDays,CreateDate")] TravelPlan travelPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(travelPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", travelPlan.MemberId);
            return View(travelPlan);
        }

        // GET: TravelPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TravelPlans == null)
            {
                return NotFound();
            }

            var travelPlan = await _context.TravelPlans.FindAsync(id);
            if (travelPlan == null)
            {
                return NotFound();
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", travelPlan.MemberId);
            return View(travelPlan);
        }

        // POST: TravelPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,TravelDays,CreateDate")] TravelPlan travelPlan)
        {
            if (id != travelPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(travelPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TravelPlanExists(travelPlan.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", travelPlan.MemberId);
            return View(travelPlan);
        }

        // GET: TravelPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TravelPlans == null)
            {
                return NotFound();
            }

            var travelPlan = await _context.TravelPlans
                .Include(t => t.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (travelPlan == null)
            {
                return NotFound();
            }

            return View(travelPlan);
        }

        // POST: TravelPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TravelPlans == null)
            {
                return Problem("Entity set 'RouteMasterContext.TravelPlans'  is null.");
            }
            var travelPlan = await _context.TravelPlans.FindAsync(id);
            if (travelPlan != null)
            {
                _context.TravelPlans.Remove(travelPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TravelPlanExists(int id)
        {
          return (_context.TravelPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
