using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;

namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomProductsController : ControllerBase
    {
        private readonly RouteMasterContext _db;

        public RoomProductsController(RouteMasterContext context)
        {
            _db = context;
        }

        // GET: api/RoomProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomProduct>>> GetRoomProducts()
        {
          if (_db.RoomProducts == null)
          {
              return NotFound();
          }
            return await _db.RoomProducts.ToListAsync();
        }

        // GET: api/RoomProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomProductsDTO>> GetRoomProduct(int id)
        {
          if (_db.RoomProducts == null)
          {
              return NotFound();
          }
            var roomProduct = _db.RoomProducts.Where(rp => rp.Date > DateTime.Now.AddDays(-1) && rp.RoomId == id);
			if (roomProduct == null)
            {
                return NotFound();
            }

            roomProduct = roomProduct.FirstOrDefault().Date.AddHours(18) < DateTime.Now ? roomProduct.Skip(1) : roomProduct;
            var ablerp = roomProduct.Where(rp => rp.Quantity > 0);
            var disablerp = roomProduct.Where(rp => rp.Quantity == 0);


            var dto  = new RoomProductsDTO();
            dto.Items = await ablerp.Select(rp => new RoomProductDTOItem
			{
                Id = rp.Id,
				Date = new string($"{rp.Date.Year}-{rp.Date.Month:D2}-{rp.Date.Day:D2}"),
				NewPrice = rp.NewPrice,
				Quantity = rp.Quantity,
			}).ToListAsync();
            dto.DisableDate = await disablerp.Select(drp=> new string($"{drp.Date.Year}-{drp.Date.Month:D2}-{drp.Date.Day:D2}")).ToListAsync();

            var data = new JsonResult(dto);

			return dto;
        }

        // PUT: api/RoomProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomProduct(int id, RoomProduct roomProduct)
        {
            if (id != roomProduct.Id)
            {
                return BadRequest();
            }

            _db.Entry(roomProduct).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RoomProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomProduct>> PostRoomProduct(RoomProduct roomProduct)
        {
          if (_db.RoomProducts == null)
          {
              return Problem("Entity set 'RouteMasterContext.RoomProducts'  is null.");
          }
            _db.RoomProducts.Add(roomProduct);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetRoomProduct", new { id = roomProduct.Id }, roomProduct);
        }

        // DELETE: api/RoomProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomProduct(int id)
        {
            if (_db.RoomProducts == null)
            {
                return NotFound();
            }
            var roomProduct = await _db.RoomProducts.FindAsync(id);
            if (roomProduct == null)
            {
                return NotFound();
            }

            _db.RoomProducts.Remove(roomProduct);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomProductExists(int id)
        {
            return (_db.RoomProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
