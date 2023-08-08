using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;

namespace RouteMasterBackend.Controllers

{
    [EnableCors("AllowAny")]
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

        //仿造response
        [HttpGet]
        public  ActionResult<IEnumerable<InformationDto>> GetInformations()
        {

            var data = new List<InformationDto>
            {
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    Start=new Location{Id=1,Name="龜山島"},
                    End=new Location{Id=1,Name="龜山島"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="137 公里",Value=136581},
                    Duration=new Duration{Text="2 小時 49 分鐘",Value=10127},
                    Start=new Location{Id=1,Name="龜山島"},
                    End=new Location{Id=2,Name="遠雄海洋公園"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="126 公里",Value=126410},
                    Duration=new Duration{Text="2 小時 35 分鐘",Value=9295},
                    Start=new Location{Id=1,Name="龜山島"},
                    End=new Location{Id=3,Name="太魯閣"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="53.6 公里",Value=53603},
                    Duration=new Duration{Text="1 小時 1 分鐘",Value=3677},
                    Start=new Location{Id=1,Name="龜山島"},
                    End=new Location{Id=4,Name="故宮博物院"}
                },

                new InformationDto
                {
                    Distance=new Distance{Text="53.6 公里",Value=53603},
                    Duration=new Duration{Text="1 小時 1 分鐘",Value=3677},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                    End=new Location{Id=1,Name="龜山島"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                    End=new Location{Id=2,Name="遠雄海洋公園"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="12.5 公里",Value=12454},
                    Duration=new Duration{Text="19 分鐘",Value=1113},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                    End=new Location{Id=3,Name="太魯閣"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="175 公里",Value=174978},
                    Duration=new Duration{Text="3 小時 19 分鐘",Value=11945},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                    End=new Location{Id=4,Name="故宮博物院"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="126 公里",Value=125627},
                    Duration=new Duration{Text="2 小時 33 分鐘",Value=9152},
                    Start=new Location{Id=3,Name="太魯閣"},
                    End=new Location{Id=1,Name="龜山島"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="12.0 公里",Value=11954},
                    Duration=new Duration{Text="18 分鐘",Value=1068},
                    Start=new Location{Id=3,Name="太魯閣"},
                    End=new Location{Id=2,Name="遠雄海洋公園"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    Start=new Location{Id=3,Name="太魯閣"},
                    End=new Location{Id=3,Name="太魯閣"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="164 公里",Value=164188},
                    Duration=new Duration{Text="3 小時 6 分鐘",Value=11162},
                    Start=new Location{Id=3,Name="太魯閣"},
                    End=new Location{Id=4,Name="故宮博物院"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="54.8 公里",Value=54785},
                    Duration=new Duration{Text="1 小時 3 分鐘",Value=3773},
                    Start=new Location{Id=4,Name="故宮博物院"},
                    End=new Location{Id=1,Name="龜山島"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="176 公里",Value=175541},
                    Duration=new Duration{Text="3 小時 23 分鐘",Value=12190},
                    Start=new Location{Id=4,Name="故宮博物院"},
                    End=new Location{Id=2,Name="遠雄海洋公園"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="165 公里",Value=165369},
                    Duration=new Duration{Text="3 小時 9 分鐘",Value=11357},
                    Start=new Location{Id=4,Name="故宮博物院"},
                    End=new Location{Id=3,Name="太魯閣"}
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    Start=new Location{Id=4,Name="故宮博物院"},
                    End=new Location{Id=4,Name="故宮博物院"}
                },
            };



            return data;
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
