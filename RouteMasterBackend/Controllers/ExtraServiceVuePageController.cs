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
        public async Task<ExtraServicePagingVueDto> FilterExtraServices(ExtraServiceCriteria criteria)
        {
            var data = _context.ExtraServices.Include(x=>x.Region).Include(x=>x.Attraction).AsQueryable();
            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                data = data.Where(x => x.Name.Contains(criteria.Keyword));
            }



            int totalCount = data.Count();

            int totalPages = (int)Math.Ceiling(totalCount / (double)criteria.PageSize);

            data = data.Skip(criteria.PageSize * (criteria.Page - 1)).Take(criteria.PageSize);



            var resultData=  data.Select(x => new ExtraServiceVuePageIndexDto
            {
              Id= x.Id,
              Name= x.Name,
              Status=x.Status,
              Image= "/ExtraServiceImages/"+x.Image,   
              Description= x.Description,
              RegionName=x.Region.Name,
              AttractionName=x.Attraction.Name
            });



            ExtraServicePagingVueDto extraServicePagingDto=new ExtraServicePagingVueDto();
            extraServicePagingDto.ExtraServiceVuePageDtoes=await resultData.ToListAsync();
            extraServicePagingDto.TotalPage = totalPages;   

            return extraServicePagingDto;
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
        [HttpPost("getProducts")]
        public async Task<IEnumerable<ExtraServiceProductInCalenderDto>>GetProducts(ExtraServiceProductSelectCriteria criteria)
        {
            var extProductsInDb=_context.ExtraServiceProducts.Where(x=>x.ExtraServiceId==criteria.ExtraServiceId).Where(x=>x.Date.Year==criteria.CurrentYear&&x.Date.Month==criteria.CurrentMonth);

            var resultData = extProductsInDb.Select(x => new ExtraServiceProductInCalenderDto
            {
                Id=x.Id,
                Date=x.Date,
                Price=x.Price,
                Quantity=x.Quantity,    
            });
            
            return resultData;
        }

    }
    
}
