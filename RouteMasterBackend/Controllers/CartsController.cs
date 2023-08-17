
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;

namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly Models.RouteMasterContext _context;

        public CartsController(Models.RouteMasterContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            int cartIdFromCookie = Convert.ToInt32(Request.Cookies["CartId"] ?? "0");

                var cartDetailsDto = new CartDetailDto
                {
                    ExtraServices = _context.CartExtraServicesDetails
                   .Where(c => c.CartId == cartIdFromCookie)
                   .Include(c => c.ExtraServiceProduct)
                   .Include(c => c.ExtraServiceProduct.ExtraService)
                   .Select(cartDetail => new Cart_ExtraServicesDetailDto
                   {
                       Id = cartDetail.Id,
                       Name = cartDetail.ExtraServiceProduct.ExtraService.Name,
                       Description = cartDetail.ExtraServiceProduct.ExtraService.Description,
                       Price = cartDetail.ExtraServiceProduct.Price,
                       Date = cartDetail.ExtraServiceProduct.Date,
                       Quantity = cartDetail.Quantity,
                       ImageUrl = "/ExtraServiceImages/" + cartDetail.ExtraServiceProduct.ExtraService.Image,
                   })
                    .ToList(),

                    Accommodations = _context.CartAccommodationDetails
                    .Where(c => c.CartId == cartIdFromCookie)
                    .Include(c => c.RoomProduct)
                    .Include(c => c.RoomProduct.Room)
                    .Include(c => c.RoomProduct.Room.Accommodation)
                    .Include(c => c.RoomProduct.Room.RoomType)
                    .Select(cartDetail => new Cart_AccommodationDetailDto
                    {
                        Id = cartDetail.Id,
                        RoomName = cartDetail.RoomProduct.Room.Name,
                        AccommodationName = cartDetail.RoomProduct.Room.Accommodation.Name,
                        RoomTypeName = cartDetail.RoomProduct.Room.RoomType.Name,
                        Price = cartDetail.RoomProduct.NewPrice,
                        Date = cartDetail.RoomProduct.Date,
                        Quantity = cartDetail.Quantity,
                        ImageUrl = "123"
                    })
                        .ToList(),

                    Activities = _context.CartActivitiesDetails
                       .Where(c => c.CartId == cartIdFromCookie)
                       .Include(c => c.ActivityProduct)
                       .Include(c => c.ActivityProduct.Activity)
                       .Select(cartDetail => new Cart_ActivitiesDetailDto
                       {
                           Id = cartDetail.Id,
                           ActivityName = cartDetail.ActivityProduct.Activity.Name,
                           Description = cartDetail.ActivityProduct.Activity.Description,
                           Price = cartDetail.ActivityProduct.Price,
                           StartTime = cartDetail.ActivityProduct.StartTime,
                           EndTime = cartDetail.ActivityProduct.EndTime,
                           Quantity = cartDetail.Quantity,
                           ImageUrl = "/ActivityImages/" + cartDetail.ActivityProduct.Activity.Image,
                       })
                       .ToList()
                };
                return Ok(new { CartId = cartIdFromCookie, CartDetails = cartDetailsDto });

        }

        [HttpPost("addextraservice")]
        public IActionResult AddExtraService2Cart([FromBody]AddExtraServiceDto dto)
        {
            try
            {
                var extraServiceProduct = _context.ExtraServiceProducts
                    .FirstOrDefault(p => p.Id == dto.extraserviceId);

                var cartItem = new CartExtraServicesDetail
                {
                    CartId = dto.cartId,
                    ExtraServiceProductId = dto.extraserviceId,
                    Quantity = dto.quantity,
                };

                _context.CartExtraServicesDetails.Add(cartItem);
                _context.SaveChanges();
 

                return Ok(new { success = true, message = "Successfully added to cart." });
            }

            catch(Exception ex)
            {
               
                return BadRequest(new { success = false, message = "Failed to add to cart.", error = ex.Message });
            }
        }
        [HttpPost("addactivity")]
        public IActionResult AddActivitiesDetail2Cart([FromBody]AddActivityDto dto)
        {
            try
            {
                var activitiesProduct = _context.ActivityProducts.FirstOrDefault(p => p.Id == dto.activityid);

                //if(activitiesProduct == null)
                //{
                //    return NotFound(new { success = false, message = "Product Not found." });
                //}
               //var cartIdFromCookie = Convert.ToInt32(HttpContext.Request.Cookies["CartId"] ?? "0");
               var cartItem = new CartActivitiesDetail
                {
                    CartId = dto.cartId,
                    ActivityProductId = dto.activityid,
                    Quantity = dto.quantity,
                };
                _context.CartActivitiesDetails.Add(cartItem);
                _context.SaveChanges();
              

                return Ok(new { success = true, message = "Succesfully added to cart." });
            }
            catch(Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to add to cart.", error = ex.Message });
            }
        }

        [HttpPost("addAccommodation")]
        public IActionResult AddAccommodation2Cart([FromBody]AddAccommodationDto dto)
        {
            try
            {
                var roomProduct = _context.RoomProducts.FirstOrDefault(p => p.Id == dto.roomproductId);
                if(roomProduct == null)
                {
                    return NotFound(new { success = false, message = "Product Not Found." });
                }
                var cartitem = new CartAccommodationDetail
                {
                    CartId = dto.cartId,
                    RoomProductId = dto.roomproductId,
                    Quantity = dto.quantity,
                };
                _context.CartAccommodationDetails.Add(cartitem);
                _context.SaveChanges();
                return Ok(new { success = true, message = "Succesfully added to cart." });

            }
            catch(Exception ex) 
            {
                return BadRequest(new { success = false, message = "Failed to add to cart.", error = ex.Message });
            }
        }

        [HttpPut("updateExtraServicequantity")]
        public IActionResult UpdateExtraServiceQuantity([FromBody] UpdateExtQuantityDto dto)
        {
            try
            {
              
                var cartItem = _context.CartExtraServicesDetails
                    .FirstOrDefault(item => item.CartId == dto.CartId && item.ExtraServiceProductId == dto.ExtraServiceProductId);

                if (cartItem == null)
                {
                    return NotFound(new { success = false, message = "Cart item not found." });
                }

                cartItem.Quantity = dto.Quantity;
                _context.SaveChanges();

                return Ok(new { success = true, message = "Successfully updated quantity." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to update quantity.", error = ex.Message });
            }
        }
        [HttpPut("updateActivityquantity")]
        public IActionResult UpdateActivityQuantity([FromBody] UpdateActQuantityDto dto)
        {
            try
            {

                var cartItem = _context.CartActivitiesDetails
                    .FirstOrDefault(item => item.CartId == dto.CartId && item.ActivityProductId == dto.ActivityProductId);

                if (cartItem == null)
                {
                    return NotFound(new { success = false, message = "Cart item not found." });
                }

                cartItem.Quantity = dto.Quantity;
                _context.SaveChanges();

                return Ok(new { success = true, message = "Successfully updated quantity." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to update quantity.", error = ex.Message });
            }
        }
        [HttpPut("updateAccommodationquantity")]
        public IActionResult UpdateAccommodationQuantity([FromBody] UpdateAccoQuantityDto dto)
        {
            try
            {

                var cartItem = _context.CartAccommodationDetails
                    .FirstOrDefault(item => item.CartId == dto.CartId && item.RoomProductId == dto.RoomProductId);

                if (cartItem == null)
                {
                    return NotFound(new { success = false, message = "Cart item not found." });
                }

                cartItem.Quantity = dto.Quantity;
                _context.SaveChanges();

                return Ok(new { success = true, message = "Successfully updated quantity." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to update quantity.", error = ex.Message });
            }
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
