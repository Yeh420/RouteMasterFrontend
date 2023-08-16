using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;
using SQLitePCL;

namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraServiceVuePageController : ControllerBase
    {
        private readonly RouteMasterContext _context;
        public ExtraServiceVuePageController(RouteMasterContext context)
        {
            _context = context;
            
        }


        [HttpPost("filter")]
        public async Task<IEnumerable<ExtraServiceVuePageIndexDto>> FilterExtraServices(ExtraServiceCriteria criteria)
        {
            var data = _context.ExtraServices.Include(x=>x.Region).Include(x=>x.Attraction).AsQueryable();
            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                data = data.Where(x => x.Name.Contains(criteria.Keyword));
            }




          var resultData=  data.Select(x => new ExtraServiceVuePageIndexDto
            {
              Id= x.Id,
              Name= x.Name,
              Image= "/ExtraServiceImages/"+x.Image,   
              Description= x.Description,
              RegionName=x.Region.Name,
              AttractionName=x.Attraction.Name
            });


            return resultData;
        }


        [HttpPost("Id")]
        public async Task<ExtraServiceVuePageIndexDto> GetTargetExtraService(int extraServiceId)
        {
            var data = _context.ExtraServices.Include(x => x.Region).Include(x => x.Attraction).Where(x=>x.Id==extraServiceId).First();


            var resultData = new ExtraServiceVuePageIndexDto
            {
                Id = data.Id,
                Name = data.Name,
                Image = "/ExtraServiceImages/" + data.Image,
                Description = data.Description,
                RegionName = data.Region.Name,
                AttractionName =data.Attraction.Name
            };

           





            return resultData;
        }


    }
    
}
