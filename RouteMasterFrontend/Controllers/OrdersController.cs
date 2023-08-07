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
    public class OrdersController : Controller
    {
        private readonly RouteMasterContext _context;

        public OrdersController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var routeMasterContext = _context.Orders.Include(o => o.Coupons).Include(o => o.Member).Include(o => o.OrderHandleStatus).Include(o => o.PaymentMethod).Include(o => o.PaymentStatus);
            return View(await routeMasterContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CouponsId"] = new SelectList(_context.Coupons, "Id", "Id");
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Account");
            ViewData["OrderHandleStatusId"] = new SelectList(_context.OrderHandleStatuses, "Id", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Description");
            ViewData["PaymentStatusId"] = new SelectList(_context.PaymentStatuses, "Id", "Name");
            ViewData["TravelPlanId"] = new SelectList(_context.TravelPlans, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MemberId,TravelPlanId,PaymentMethodId,PaymentStatusId,OrderHandleStatusId,CouponsId,CreateDate,ModifiedDate,Total")] Order order)
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

        // GET: Orders/Edit/5
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

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MemberId,TravelPlanId,PaymentMethodId,PaymentStatusId,OrderHandleStatusId,CouponsId,CreateDate,ModifiedDate,Total")] Order order)
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

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
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
