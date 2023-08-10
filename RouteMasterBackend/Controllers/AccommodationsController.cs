using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    public class AccommodationsController : ControllerBase
    {
        private readonly RouteMasterContext _context;

        public AccommodationsController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: api/Accommodations
        [HttpGet]
        public async Task<ActionResult<AccommodtaionsDTO>> GetAccommodations(string? keyword, int page = 1, int pageSize = 3)
        {
            
            if (_context.Accommodations == null)
            {
                return NotFound();
            }
            var accommodations = _context.Accommodations
                    .Include(a => a.CommentsAccommodations)
                    .Include(a => a.AccommodationImages)
                    .Include(a => a.Rooms)
                    .Include(a => a.AccommodationServiceInfos).AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
	            accommodations = accommodations.Where(p => 
                    p.Name.Contains(keyword)||
                    p.Description.Contains(keyword)||
                    p.Address.Contains(keyword)||
                    p.AccommodationServiceInfos.Where(s=>s.Name.Contains(keyword)).Any()
                );

            }
            #region
            //分頁
            int totalCount = accommodations.Count(); //總共幾筆 ex:10
            int totalPage = (int)Math.Ceiling(totalCount / (double)pageSize); //計算總共幾頁 ex:4

            accommodations = accommodations.Skip(pageSize * (page - 1)).Take(pageSize);
            //page = 0*3 take 1,2,3
            //page = 1*3 take 4,5,6
            //page = 2*3 take 7,8,9
            #endregion
            AccommodtaionsDTO accommodationsDTO = new AccommodtaionsDTO();
            accommodationsDTO.Items = await accommodations.Select(a => new AccommodtaionsDTOItem
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Grade = a.Grade,
                Address = a.Address,
                PositionX = a.PositionX,
                PositionY = a.PositionY,
                Website = a.Website,
                IndustryEmail = a.IndustryEmail,
                PhoneNumber = a.PhoneNumber,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                Images = a.AccommodationImages,
                Comments = a.CommentsAccommodations,
                Rooms = a.Rooms,
                Services = a.AccommodationServiceInfos
            }).ToListAsync();
            accommodationsDTO.TotalPages = totalPage;

            return accommodationsDTO;
        }

        // GET: api/Accommodations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Accommodation>> GetAccommodation(int id)
        {
          if (_context.Accommodations == null)
          {
              return NotFound();
          }
            var accommodation = await _context.Accommodations.FindAsync(id);

            if (accommodation == null)
            {
                return NotFound();
            }

            return accommodation;
        }

        [HttpGet("/Filter")]
         public async Task<ActionResult<FilterDTO>> GetFilterDTO()
        {
            return new FilterDTO();
        }
        // PUT: api/Accommodations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccommodation(int id, Accommodation accommodation)
        {
            if (id != accommodation.Id)
            {
                return BadRequest();
            }

            _context.Entry(accommodation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(id))
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

        // POST: api/Accommodations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Accommodation>> PostAccommodation(Accommodation accommodation)
        {
          if (_context.Accommodations == null)
          {
              return Problem("Entity set 'RouteMasterContext.Accommodations'  is null.");
          }
            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccommodation", new { id = accommodation.Id }, accommodation);
        }

        // DELETE: api/Accommodations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccommodation(int id)
        {
            if (_context.Accommodations == null)
            {
                return NotFound();
            }
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation == null)
            {
                return NotFound();
            }

            _context.Accommodations.Remove(accommodation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccommodationExists(int id)
        {
            return (_context.Accommodations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
