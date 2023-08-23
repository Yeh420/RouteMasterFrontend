using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.Controllers;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Interfaces;

namespace RouteMasterFrontend.Models.Services
{
    public class CouponService: ICoupon
    {
        private readonly RouteMasterContext _db;
        public CouponService(RouteMasterContext db)
        {
            _db = db;
        }

        public async Task<List<CouponsDto>> GetAllCouponsAsync()
        {
            var now = DateTime.Now.Date;
            var coupons = await _db.Coupons
                  .OrderBy(c => c.EndDate)
                .Select(c => new CouponsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Discount = c.Discount,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    IsActive = c.IsActive,
                }).ToListAsync();

            foreach (var coupon in coupons)
            {
                if (coupon.StartDate.Date <= now && now <= coupon.EndDate.Date && coupon.IsActive == true)
                {
                    coupon.Valuable = true;
                }
                else
                {
                    coupon.Valuable = false;
                }
            }

            return coupons;
        }
    }
}
