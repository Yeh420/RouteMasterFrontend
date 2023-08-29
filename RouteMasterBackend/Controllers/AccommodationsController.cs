using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using RouteMasterBackend.DTOs;
using RouteMasterBackend.Models;
using RouteMasterFrontend.EFModels;

namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationsController : ControllerBase
    {
        private readonly Models.RouteMasterContext _db;

        public AccommodationsController(Models.RouteMasterContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccommodationDistanceDTO>>> GetRecommendAccommodations(double lngX, double latY, int topN = 10)
        {
            var datas = await _db.Accommodations
                .Include(a => a.AcommodationCategory)
                .Include(a => a.Rooms)
                .Include(a => a.CommentsAccommodations)
                .AsNoTracking()
                .Select(data => new AccommodationDistanceDTO
                {
                    Id = data.Id,
                    ACategory = data.AcommodationCategory.Name,
                    Name = data.Name,
                    Grade = data.Grade,
                    Score = data.CommentsAccommodations.Average(ca => ca.Score) > 0 ? data.CommentsAccommodations.Average(ca => ca.Score).ToString("0.0") : "0",
                    Address = data.Address,
                    PositionX = data.PositionX,
                    PositionY = data.PositionY,
                    Image = data.Image,
                    Comment = data.CommentsAccommodations.Count(),
                    Distance = Math.Sqrt(
                Math.Pow((double)(lngX - data.PositionX), 2) +
                Math.Pow((double)(latY - data.PositionY), 2)) * 111,
                    Rooms = data.Rooms.Select(r=> new AccommodationDistanceRDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Price = r.Price
                    })
                })
                .OrderBy(a => Math.Sqrt(
                Math.Pow((double)(lngX - a.PositionX), 2) +
                Math.Pow((double)(latY - a.PositionY), 2))).Take(topN).ToListAsync();

            return datas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccommodtaionsDTO>> GetAccommodations(int id)
        {
            var accommodations = _db.Accommodations
                    .Include(a => a.CommentsAccommodations)
                    .Include(a => a.AccommodationImages)
                    .Include(a => a.Rooms)
                    .Include(a => a.AccommodationServiceInfos)
                    .Where(a => a.Id == id);

            var dto = await accommodations.Select(a=>new AccommodtaionsDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Grade = a.Grade,
                Address = a.Address,
                PositionX = a.PositionX,
                PositionY = a.PositionY,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                Score = a.CommentsAccommodations.Average(ca => ca.Score) > 0 ? a.CommentsAccommodations.Average(ca => ca.Score).ToString("0.0") : "0",
                Comments = a.CommentsAccommodations.Count(),
                Image = a.Image,
                Images = a.AccommodationImages,
                Rooms = a.Rooms,
                Services = a.AccommodationServiceInfos
            }).FirstAsync();
            
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<AccommodtaionsInfoDTO>> GetAccommodationInfos(FilterData data)
        {

            if (_db.Accommodations == null)
            {
                return NotFound();
            }
            var accommodations = _db.Accommodations.AsNoTracking()
                    .Include(a => a.AcommodationCategory)
                    .Include(a => a.CommentsAccommodations)
                    .Include(a => a.AccommodationServiceInfos)
                    .Include(a => a.Rooms)
                    .ThenInclude(r => r.RoomProducts)
                    .AsQueryable();

            if(data.MinBudget > 0)
            {
                accommodations = accommodations.Where(a => a.Rooms.Min(r =>r.Price)>= data.MinBudget);
            }

            if(data.MaxBudget < 10000)
            {
                accommodations = accommodations.Where(a => a.Rooms.Min(r => r.Price) <= data.MaxBudget);
            }

            if (data.Keyword != null && data.Keyword.Length != 0)
            {
                foreach (var keyword in data.Keyword)
                {
                    accommodations = accommodations.Where(a =>
                    a.Name.Contains(keyword) ||
                    a.Description.Contains(keyword) ||
                    a.Address.Contains(keyword) ||
                    a.AccommodationServiceInfos.Where(s => s.Name.Contains(keyword)).Any()
                    );
                }
            }

            if (data.Grades != null && data.Grades.Length > 0)
            {
                accommodations = accommodations.Where(a => data.Grades.Contains(a.Grade));
            };

            if (data.ACategory != null && data.ACategory.Length > 0)
            {
                accommodations = accommodations.Where(a => data.ACategory.Contains(a.AcommodationCategory.Name));
            };
            if (data.score != null)
            {
                accommodations = accommodations.Where(a => a.CommentsAccommodations.Average(ca => ca.Score) >= data.score);
            }

            if (data.SCategory != null && data.SCategory.Length > 0)
            {
                foreach (var sCategory in data.SCategory)
                {
                    accommodations = accommodations.Where(a => a.AccommodationServiceInfos.Any(s => s.Name == sCategory));
                }
            }

            if (data.Regions != null && data.Regions.Length > 0)
            {
                accommodations = accommodations.Where(a => data.Regions.Contains(a.Region.Name));
            };

            accommodations = data.SortBy switch
            {
                1 => accommodations.OrderByDescending(a => a.Grade),
                2 => accommodations.OrderBy(a => a.Grade),
                3 => accommodations.OrderByDescending(a => a.CommentsAccommodations.Average(ca => ca.Score)),
                4 => accommodations.OrderBy(a => a.Rooms.Min(r => r.Price)),
                5 => accommodations.OrderByDescending(a => a.Rooms.Min(r => r.Price)),
                _ => accommodations,
            };

            #region
            //分頁
            int totalCount = accommodations.Count(); //總共幾筆 ex:10
            int totalPage = (int)Math.Ceiling(totalCount / (double)data.PageSize); //計算總共幾頁 ex:4

            accommodations = accommodations.Skip(data.PageSize * (data.Page - 1)).Take(data.PageSize);
            //page = 0*3 take 1,2,3
            //page = 1*3 take 4,5,6
            //page = 2*3 take 7,8,9
            #endregion
            AccommodtaionsInfoDTO dto = new AccommodtaionsInfoDTO();
            dto.Items = await accommodations.Select(a => new AccommodtaionsDTOItem
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Grade = a.Grade,
                Address = a.Address,
                Image = a.Image,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                Score = a.CommentsAccommodations.Average(ca=>ca.Score) > 0 ? a.CommentsAccommodations.Average(ca => ca.Score).ToString("0.0") : "0",
                Comment = a.CommentsAccommodations.Count(),
                Price = a.Rooms.Count() > 0 ? a.Rooms.Min(r => r.Price) : 0,
                Services = a.AccommodationServiceInfos
            }).ToListAsync();
            dto.TotalPages = totalPage;

            return dto;
        }
        
        
        //[HttpGet("GetFilterDTO")]
        // public async Task<ActionResult<FilterDTO>> GetFilterDTO()
        //{
        //    return new FilterDTO
        //    {
        //        Grades = await _db.Accommodations.Select(a => a.Grade).Distinct().ToListAsync(),
        //        AcommodationCategories = await _db.AcommodationCategories.Select(ac=>ac.Name).ToListAsync(),
        //        ServiceInfoCategories = await _db.ServiceInfoCategories.Select(sc=>sc.Name).ToListAsync(),
        //        Regions = await _db.Regions.Select(r => r.Name).ToListAsync()
        //    };
        //}


        // PUT: api/Accommodations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAccommodation(int id, Accommodation accommodation)
        //{
        //    if (id != accommodation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(accommodation).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AccommodationExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Accommodations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Accommodation>> PostAccommodation(Accommodation accommodation)
        //{
        //  if (_db.Accommodations == null)
        //  {
        //      return Problem("Entity set 'RouteMasterContext.Accommodations'  is null.");
        //  }
        //    _db.Accommodations.Add(accommodation);
        //    await _db.SaveChangesAsync();

        //    return CreatedAtAction("GetAccommodation", new { id = accommodation.Id }, accommodation);
        //}

        // DELETE: api/Accommodations/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAccommodation(int id)
        //{
        //    if (_db.Accommodations == null)
        //    {
        //        return NotFound();
        //    }
        //    var accommodation = await _db.Accommodations.FindAsync(id);
        //    if (accommodation == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Accommodations.Remove(accommodation);
        //    await _db.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AccommodationExists(int id)
        //{
        //    return (_db.Accommodations?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
