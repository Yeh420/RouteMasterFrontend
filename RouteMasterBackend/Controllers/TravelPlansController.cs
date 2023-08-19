using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Packaging;
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
                    End=new Location{Id=1,Name="龜山島"},
                    Start=new Location{Id=1,Name="龜山島"},
                },

                new InformationDto
                {
                    Distance=new Distance{Text="137 公里",Value=136575},
                    Duration=new Duration{Text="2 小時 49 分鐘",Value=10126},
                    End=new Location{Id=2,Name="遠雄海洋公園"},
                    Start=new Location{Id=1,Name="龜山島"},
                },

                new InformationDto
                {
                    Distance=new Distance{Text="125 公里",Value=124620},
                    Duration=new Duration{Text="2 小時 35 分鐘",Value=9309},
                    End=new Location{Id=3,Name="太魯閣"},
                    Start=new Location{Id=1,Name="龜山島"},
                },


                new InformationDto
                {
                    Distance=new Distance{Text="53.6 公里",Value=53597},
                    Duration=new Duration{Text="1 小時 1 分鐘",Value=3677},
                    End=new Location{Id=4,Name="故宮博物院"},
                    Start=new Location{Id=1,Name="龜山島"},
                },







                new InformationDto
                {
                    Distance=new Distance{Text="136 公里",Value=136411},
                    Duration=new Duration{Text="2 小時 46 分鐘",Value=9934},
                    End=new Location{Id=1,Name="龜山島"},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    End=new Location{Id=2,Name="遠雄海洋公園"},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="56.5 公里",Value=56480},
                    Duration=new Duration{Text="1 小時 25 分鐘",Value=5104},
                    End=new Location{Id=3,Name="太魯閣"},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="175 公里",Value=174978},
                    Duration=new Duration{Text="3 小時 19 分鐘",Value=11945},
                    End=new Location{Id=4,Name="故宮博物院"},
                    Start=new Location{Id=2,Name="遠雄海洋公園"},
                },






                new InformationDto
                {
                    Distance=new Distance{Text="124 公里",Value=123932},
                    Duration=new Duration{Text="2 小時 33 分鐘",Value=9171},
                    End=new Location{Id=1,Name="龜山島"},
                    Start=new Location{Id=3,Name="太魯閣"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="55.6 公里",Value=55604},
                    Duration=new Duration{Text="1 小時 25 分鐘",Value=5072},
                    End=new Location{Id=2,Name="遠雄海洋公園"},
                    Start=new Location{Id=3,Name="太魯閣"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    End=new Location{Id=3,Name="太魯閣"},
                    Start=new Location{Id=3,Name="太魯閣"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="162 公里",Value=162498},
                    Duration=new Duration{Text="3 小時 6 分鐘",Value=11181},
                    End=new Location{Id=4,Name="故宮博物院"},
                    Start=new Location{Id=3,Name="太魯閣"},
                },








                new InformationDto
                {
                    Distance=new Distance{Text="54.8 公里",Value=54779},
                    Duration=new Duration{Text="1 小時 3 分鐘",Value=3772},
                    End=new Location{Id=1,Name="龜山島"},
                    Start=new Location{Id=4,Name="故宮博物院"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="176 公里",Value=175541},
                    Duration=new Duration{Text="3 小時 23 分鐘",Value=12190},
                    End=new Location{Id=2,Name="遠雄海洋公園"},
                    Start=new Location{Id=4,Name="故宮博物院"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="164 公里",Value=163586},
                    Duration=new Duration{Text="3 小時 10 分鐘",Value=11372},
                    End=new Location{Id=3,Name="太魯閣"},
                    Start=new Location{Id=4,Name="故宮博物院"},
                },
                new InformationDto
                {
                    Distance=new Distance{Text="1公尺",Value=0},
                    Duration=new Duration{Text="1分鐘",Value=0},
                    End=new Location{Id=4,Name="故宮博物院"},
                    Start=new Location{Id=4,Name="故宮博物院"},
                },
            };

           

            return data;
        }

        // GET: api/TravelPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AttractionInfoDto>> GetAttractionInfo(int id,DateTime startDateTime)
        {
            var attractionInDb= _context.Attractions.Where(a => a.Id == id).First();
            var stayHours = Math.Round(_context.CommentsAttractions.Where(c => c.AttractionId == id && c.StayHours != null).Select(c => c.StayHours.Value).Average());

            var activityProductsShowOnTravel =new List<ActivityProductShowOnTravelPlan>();
            var extraServiceProductShowOnTravel = new List<ExtraServiceProductShowOnTravelPlan>();
            

           
          
            var matchActivityIds = _context.Activities.Where(x => x.AttractionId == id).Select(x=>x.Id);
            var filterActivityProducts = _context.ActivityProducts
                .Where(x => matchActivityIds.Contains(x.ActivityId))
                .Where(x => x.Date == startDateTime.Date && x.StartTime > startDateTime.TimeOfDay)
                .Select(x => new ActivityProductShowOnTravelPlan
                {
                    Id = x.Id,
                    ActivityId = x.ActivityId,
                    ActivityName=_context.Activities.Where(a=>a.Id==x.ActivityId).First().Name,
                    Date = x.Date,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Price = x.Price,
                    Quantity = x.Quantity,
                });


            

            if (filterActivityProducts.Count() > 0)
            {
                activityProductsShowOnTravel.AddRange(filterActivityProducts);
            }
            


            var matchExtraServiceIds=_context.ExtraServices.Where(x=>x.AttractionId==id).Select(x=>x.Id);
            var filterExtraServiceProducts = _context.ExtraServiceProducts
                .Where(x => matchExtraServiceIds.Contains(x.ExtraServiceId))
                .Where(x => x.Date == startDateTime.Date)
                .Select(x => new ExtraServiceProductShowOnTravelPlan
                {
                    Id=x.Id,
                    ExtraServiceId = x.ExtraServiceId,
                    ExtraServiceName=_context.ExtraServices.Where(e=>e.Id==x.ExtraServiceId).First().Name,
                    Date=x.Date,
                    Price=x.Price,
                    Quantity = x.Quantity,  
                });


            if (filterExtraServiceProducts.Count() > 0)
            {
                extraServiceProductShowOnTravel.AddRange(filterExtraServiceProducts);
            }






            var data = new AttractionInfoDto
            {
                Id = attractionInDb.Id,
                AttractionName = attractionInDb.Name,
                PositionX = attractionInDb.PositionX,
                PositionY = attractionInDb.PositionY,
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(stayHours),
                StayHours =(int?)stayHours,
                ActivityProducts= activityProductsShowOnTravel,
                ExtraServiceProducts= extraServiceProductShowOnTravel,
            };



            return data;
         
        }


        [HttpGet]
        [Route("Get/ActProductInfo")]
        public async Task<ActionResult<ActivityProductTrDto>> GetActProductInfo(int actProductId)
        {
            var actProductInDb= _context.ActivityProducts
                .Include(x=>x.Activity).Include(x=>x.Activity.Attraction)
                .Where(x => x.Id == actProductId).First();
            var data = new ActivityProductTrDto
            {
                Id=actProductId,
                ActivityName= actProductInDb.Activity.Name,
                AttractionName= actProductInDb.Activity.Attraction.Name,
                StartTime= actProductInDb.StartTime,
                EndTime= actProductInDb.EndTime,
            };
            return data;
        }



        [HttpGet]
        [Route("Get/ExtProductInfo")]
        public async Task<ActionResult<ExtraServiceProductTrDto>> GetExtProductInfo(int extProductId)
        {
            var extProductInDb = _context.ExtraServiceProducts
                .Include(x => x.ExtraService).Include(x => x.ExtraService.Attraction)
                .Where(x => x.Id == extProductId).First();
            var data = new ExtraServiceProductTrDto
            {
                Id = extProductId,
                ExtraServiceName = extProductInDb.ExtraService.Name,
                AttractionName = extProductInDb.ExtraService.Attraction.Name,          
            };
            return data;
        }


        [HttpGet]
        [Route("Get/CalculateTransportTime")]
        public async Task<ActionResult<TransportTimeTrDto>> GetDateTime(TimeSpan latestEndTime, int timeValue)
        {
            int minuteValue=timeValue/60;
            var data = new TransportTimeTrDto
            {
                StartTime = latestEndTime,
                EndTime = latestEndTime.Add(TimeSpan.FromMinutes(minuteValue)),
            };


            return data;

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
