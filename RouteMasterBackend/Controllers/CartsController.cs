using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;

namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly RouteMasterContext _context;

        public CartsController()
        {
            _context = new RouteMasterContext();    
        }



        [HttpPost("Post/Travel")]
        public void AddItemToCart(TravelProductDto dto)
        {
              
            var cart = _context.Carts.Where(x => x.Id == dto.cartId).First();


            if(dto.activityProductIds!=null)
            {
				for (int i = 0; i < dto.activityProductIds.Length; i++)
				{
					var cartActivityDetails = new CartActivitiesDetail()
					{
						CartId = dto.cartId,
						ActivityProductId = dto.activityProductIds[i],
						Quantity = 1
					};
					cart.CartActivitiesDetails.Add(cartActivityDetails);
					_context.SaveChanges();
				}

			}
            if(dto.extraServiceProductIds!=null)
            {
				for (int i = 0; i < dto.extraServiceProductIds.Length; i++)
				{
					var cartExtraServiceDetails = new CartExtraServicesDetail()
					{
						CartId = dto.cartId,
						ExtraServiceProductId = dto.extraServiceProductIds[i],
						Quantity = 1
					};
					cart.CartExtraServicesDetails.Add(cartExtraServiceDetails);
					_context.SaveChanges();
				}
			}			


        }
    }
}
