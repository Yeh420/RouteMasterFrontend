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
    public class Orders1Controller : Controller
    {
        private readonly RouteMasterContext _context;

        public Orders1Controller(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: Orders1
        [HttpGet("Orders/Index/{memberid?}")]
        public async Task<IActionResult> Index(int memberId)
        {
          
            var routeMasterContext = _context.Orders
                 .Where(o => o.MemberId == memberId)
                .Include(o => o.Coupons)
                .Include(o => o.Member)
                .Include(o => o.OrderHandleStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.PaymentStatus)
                .Include(o => o.OrderAccommodationDetails)
                .Include(o => o.OrderActivitiesDetails)
               .Include(o => o.OrderExtraServicesDetails)
               .ToListAsync();
            ViewBag.MemberId = memberId;
            return View(await routeMasterContext);
        }

        // GET: Orders1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(x => x.OrderExtraServicesDetails)
                .Include(x => x.OrderActivitiesDetails)
                .Include(x => x.OrderAccommodationDetails)
                .Include(x => x.Coupons)
                .Include(x => x.OrderHandleStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.PaymentMethod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders1/Create
        public IActionResult Create()
        {
            ViewData["CouponsId"] = new SelectList(_context.Coupons, "Id", "Id");
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account");
            ViewData["OrderHandleStatusId"] = new SelectList(_context.OrderHandleStatuses, "Id", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Description");
            ViewData["PaymentStatusId"] = new SelectList(_context.PaymentStatuses, "Id", "Name");
            return View();
        }

        // POST: Orders1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,PaymentMethodId,PaymentStatusId,OrderHandleStatusId,CouponsId,CreateDate,ModifiedDate,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CouponsId"] = new SelectList(_context.Coupons, "Id", "Id", order.CouponsId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", order.MemberId);
            ViewData["OrderHandleStatusId"] = new SelectList(_context.OrderHandleStatuses, "Id", "Name", order.OrderHandleStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Description", order.PaymentMethodId);
            ViewData["PaymentStatusId"] = new SelectList(_context.PaymentStatuses, "Id", "Name", order.PaymentStatusId);
            return View(order);
        }

        // GET: Orders1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CouponsId"] = new SelectList(_context.Coupons, "Id", "Id", order.CouponsId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", order.MemberId);
            ViewData["OrderHandleStatusId"] = new SelectList(_context.OrderHandleStatuses, "Id", "Name", order.OrderHandleStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Description", order.PaymentMethodId);
            ViewData["PaymentStatusId"] = new SelectList(_context.PaymentStatuses, "Id", "Name", order.PaymentStatusId);
            return View(order);
        }

        // POST: Orders1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,PaymentMethodId,PaymentStatusId,OrderHandleStatusId,CouponsId,CreateDate,ModifiedDate,Total")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CouponsId"] = new SelectList(_context.Coupons, "Id", "Id", order.CouponsId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account", order.MemberId);
            ViewData["OrderHandleStatusId"] = new SelectList(_context.OrderHandleStatuses, "Id", "Name", order.OrderHandleStatusId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Description", order.PaymentMethodId);
            ViewData["PaymentStatusId"] = new SelectList(_context.PaymentStatuses, "Id", "Name", order.PaymentStatusId);
            return View(order);
        }

        // GET: Orders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Coupons)
                .Include(o => o.Member)
                .Include(o => o.OrderHandleStatus)
                .Include(o => o.PaymentMethod)
                .Include(o => o.PaymentStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'RouteMasterContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
