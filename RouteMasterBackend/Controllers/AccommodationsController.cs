using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterBackend.Models;

namespace RouteMasterBackend.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Accommodation>>> GetAccommodations()
        {
          if (_context.Accommodations == null)
          {
              return NotFound();
          }
            return await _context.Accommodations.ToListAsync();
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
