using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;

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


        public string PayInfo(string? orderId)
        {
            //Order order = GetOrderDetailsById(orderId);
            //if(order == null)
            //{
            //    return View("Error");
            //}
            //return RedirectToAction("Index", "Orders");
            return "12";
        }

        private Order GetOrderDetailsById(int orderId)
        {
            throw new NotImplementedException();
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
        public async Task<IActionResult> HistoryOrder(int? memberid)
        {

            if (!memberid.HasValue)
            {
                return BadRequest("Member ID is required");
            }
            var memberId = await _context.Members.Where(m => m.Id == memberid.Value).Select(m => m.Id).FirstOrDefaultAsync();

            if (memberId == 0)
            {
                return NotFound("Member not found");
            }

            var orders = await _context.Orders
            .Where(o => o.MemberId == memberId)
            .Include(x => x.OrderExtraServicesDetails)
            .Include(x => x.OrderActivitiesDetails)
            .Include(x => x.OrderAccommodationDetails)
            .Include(x => x.Coupons)
            .Include(x => x.OrderHandleStatus)
            .Include(x => x.PaymentStatus)
            .Include(x => x.PaymentMethod)
            .ToListAsync();

            if (!orders.Any())
            {
                return NotFound("Member not found or no orders for this member");
            }
            var orderDTOs = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                MemberId = o.MemberId,
                PaymentMethodName = o.PaymentMethod.Name,
                PaymentStatusName = o.PaymentStatus.Name,
                OrderHandleStatusId = o.OrderHandleStatusId,
                CouponsId = o.CouponsId,
                CreateDate = o.CreateDate,
                ModifiedDate = o.ModifiedDate,
                Total = o.Total,

                ExtraServiceDetails = o.OrderExtraServicesDetails.Select(es => new OrderExtraServiceDetailDTO
                {
                    Id = es.Id,
                    OrderId = es.OrderId,
                    ExtraServiceId = es.ExtraServiceId,
                    ExtraServiceName = es.ExtraServiceName,
                    ExtraServiceProductId = es.ExtraServiceProductId,

                    Date = es.Date,
                    Price = es.Price,
                    Quantity = es.Quantity
                }).ToList(),

                ActivityDetails = o.OrderActivitiesDetails.Select(ad => new OrderActivityDetailDTO
                {
                    Id = ad.Id,
                    OrderId = ad.OrderId,
                    ActivityId = ad.ActivityId,
                    ActivityName = ad.ActivityName,
                    ActivityProductId = ad.ActivityProductId,

                    Date = ad.Date,
                    StartTime = ad.StartTime,
                    EndTime = ad.EndTime,
                    Price = ad.Price,
                    Quantity = ad.Quantity
                }).ToList(),

                AccommodationDetails = o.OrderAccommodationDetails.Select(ac => new OrderAccommodationDetailDTO
                {
                    Id = ac.Id,
                    OrderId = ac.OrderId,
                    AccommodationId = ac.AccommodationId,
                    AccommodationName = ac.AccommodationName,
                    RoomProductId = ac.RoomProductId,
                    RoomType = ac.RoomType,
                    RoomName = ac.RoomName,
                    CheckIn = (DateTime)ac.CheckIn,
                    CheckOut = (DateTime)ac.CheckOut,
                    RoomPrice = ac.RoomPrice,
                    Quantity = ac.Quantity,
                    Note = ac.Note
                }).ToList()
            }).ToList();

            return View(orderDTOs);
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
