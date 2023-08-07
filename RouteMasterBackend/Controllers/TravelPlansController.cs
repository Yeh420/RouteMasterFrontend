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
    public class TravelPlansController : ControllerBase
    {
        private readonly RouteMasterContext _context;

        public TravelPlansController(RouteMasterContext context)
        {
            _context = context;
        }

        // GET: api/TravelPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelPlan>>> GetTravelPlans()
        {
          if (_context.TravelPlans == null)
          {
              return NotFound();
          }
            return await _context.TravelPlans.ToListAsync();
        }

        // GET: api/TravelPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelPlan>> GetTravelPlan(int id)
        {
          if (_context.TravelPlans == null)
          {
              return NotFound();
          }
            var travelPlan = await _context.TravelPlans.FindAsync(id);

            if (travelPlan == null)
            {
                return NotFound();
            }

            return travelPlan;
        }

        // PUT: api/TravelPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelPlan(int id, TravelPlan travelPlan)
        {
            if (id != travelPlan.Id)
            {
                return BadRequest();
            }

            _context.Entry(travelPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelPlanExists(id))
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

        // POST: api/TravelPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TravelPlan>> PostTravelPlan(TravelPlan travelPlan)
        {
          if (_context.TravelPlans == null)
          {
              return Problem("Entity set 'RouteMasterContext.TravelPlans'  is null.");
          }
            _context.TravelPlans.Add(travelPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelPlan", new { id = travelPlan.Id }, travelPlan);
        }

        // DELETE: api/TravelPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelPlan(int id)
        {
            if (_context.TravelPlans == null)
            {
                return NotFound();
            }
            var travelPlan = await _context.TravelPlans.FindAsync(id);
            if (travelPlan == null)
            {
                return NotFound();
            }

            _context.TravelPlans.Remove(travelPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelPlanExists(int id)
        {
            return (_context.TravelPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
