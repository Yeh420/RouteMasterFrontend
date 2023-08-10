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
    public class PaymentMethodsController : Controller
    {

        private readonly IConfiguration _configuration;
        private const string PaymentApiUrl = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
        private const string MerchantID = "3002607"; // 替換為您的特店編號
        private const string HashKey = "pwFHCqoQZGmho4w6"; // 替換為您的 HashKey
        private const string HashIV = "EkRm7iFT261dpevs"; // 替換為您的 HashIV
        private readonly RouteMasterContext _context;

        public PaymentMethodsController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: PaymentMethods
        public async Task<IActionResult> Index()
        {
              return _context.PaymentMethods != null ? 
                          View(await _context.PaymentMethods.ToListAsync()) :
                          Problem("Entity set 'RouteMasterContext.PaymentMethods'  is null.");
        }

        // GET: PaymentMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaymentMethods == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            return View(paymentMethod);
        }

        // GET: PaymentMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Description")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentMethod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PaymentMethods == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Description")] PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentMethod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodExists(paymentMethod.Id))
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
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PaymentMethods == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PaymentMethods == null)
            {
                return Problem("Entity set 'RouteMasterContext.PaymentMethods'  is null.");
            }
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod != null)
            {
                _context.PaymentMethods.Remove(paymentMethod);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentMethodExists(int id)
        {
          return (_context.PaymentMethods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
