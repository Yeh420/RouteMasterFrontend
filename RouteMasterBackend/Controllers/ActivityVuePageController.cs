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
    public class ActivityVuePageController : ControllerBase
    {

        private readonly RouteMasterContext _context;

        public ActivityVuePageController(RouteMasterContext context)
        {
            _context = context;             
        }

        [HttpPost("filter")]
        public async Task<IEnumerable<ActivityVuePageIndexDto>> FilterActivity(ActivityCriteria criteria)
        {
            var data = _context.Activities.Include(x => x.Region).Include(x => x.Attraction).AsQueryable();

            if(!string.IsNullOrEmpty(criteria.Keyword))
            {
               data=data.Where(x=>x.Name.Contains(criteria.Keyword));   
            }


            var resultData = data.Select(x => new ActivityVuePageIndexDto
            {
                Id = x.Id,
                Name = x.Name,
                Image = "/ActivityImages/" + x.Image,
                Description = x.Description,
                RegionName = x.Region.Name,
                AttractionName = x.Attraction.Name
            });




            return resultData;
        }




        [HttpPost("Id")]
        public async Task<ActivityVuePageIndexDto> GetTargetActivity(int activityId)
        {
            var data = _context.Activities.Include(x => x.Region).Include(x => x.Attraction).Where(x => x.Id == activityId).First();


            var resultData = new ActivityVuePageIndexDto
            {
                Id = data.Id,
                Name = data.Name,
                Image = "/ActivityImages/" + data.Image,
                Description = data.Description,
                RegionName = data.Region.Name,
                AttractionName = data.Attraction.Name
            };
            return resultData;
        }
        [HttpPost("getProducts")]
        public async Task<IEnumerable<ActivityProductInCalenderDto>> GetProducts(ActivityProductSelectCriteria criteria)
        {
            var actProductsInDb = _context.ActivityProducts.Where(x => x.ActivityId == criteria.ActivityId).Where(x => x.Date.Year == criteria.CurrentYear && x.Date.Month == criteria.CurrentMonth);

            var resultData = actProductsInDb.Select(x => new ActivityProductInCalenderDto
            {
                Id = x.Id,
                Date = x.Date,
                StartTime=x.StartTime,
                EndTime=x.EndTime,  
                Price = x.Price,
                Quantity = x.Quantity,
            });

            return resultData;
        }
    }
}
