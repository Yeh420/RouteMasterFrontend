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
using RouteMasterContext = RouteMasterBackend.Models.RouteMasterContext;

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
        public async Task<ActionResult<List<AccommodationDistanceDTO>>> GetServiceInfoCategory(double lngX, double latY, int? topN = 10)
        {
            using (var connection = _db.Database.GetDbConnection())
            {
                //connection.Open();

                // // 建立原點的地理座標
                // SqlGeography origin = SqlGeography.Point(latY, lngX, 4326);

                // // 執行地理查詢
                // string query = @"
                // SELECT TOP (@TopN) *, [Location].STDistance(@Origin) AS [Distance]
                // FROM [dbo].[Accommodations]
                // WHERE [Location].STDistance(@Origin) IS NOT NULL
                // ORDER BY [Location].STDistance(@Origin)";

                //var accommodations = await connection.QueryAsync<AccommodationDistanceDTO>(query, new { TopN = topN, Origin = origin });
                connection.Open();

                string query = @"
SELECT TOP (@TopN) a.*,SQRT(POWER([PositionX] - @OriginLng, 2) + POWER([PositionY] - @OriginLat, 2)) AS [Distance]
FROM [dbo].[Accommodations] a
ORDER BY [Distance]";

                var accommodations = await connection.QueryAsync<AccommodationDistanceDTO>(query, new { TopN = topN, OriginLat = latY, OriginLng = lngX });

                connection.Close();

                return accommodations.ToList();
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<AccommodtaionsDTO>> GetAccommodations(FilterData data)
        {
            
            if (_db.Accommodations == null)
            {
                return NotFound();
            }
            var accommodations = _db.Accommodations.AsNoTracking()
                    .Include(a => a.CommentsAccommodations)
                    .Include(a => a.AccommodationImages)
                    .Include(a => a.Rooms)
                    .Include(a => a.AccommodationServiceInfos)
                    .AsQueryable();
            if (!string.IsNullOrEmpty(data.Keyword))
            {
	            accommodations = accommodations.Where(p => 
                    p.Name.Contains(data.Keyword) ||
                    p.Description.Contains(data.Keyword) ||
                    p.Address.Contains(data.Keyword) ||
                    p.AccommodationServiceInfos.Where(s=>s.Name.Contains(data.Keyword)).Any()
                );
            }

            if (data.Grades != null && data.Grades.Length > 0)
            {
                accommodations = accommodations.Where(a => data.Grades.Contains(a.Grade));
            };

            if (data.ACategory != null && data.ACategory.Length > 0)
            {
                accommodations = accommodations.Where(a => data.ACategory.Contains(a.AcommodationCategory.Name));
            };
            if(data.score != null)
            {
                accommodations = accommodations.Where(a => a.CommentsAccommodations.Average(ca=>ca.Score) >= data.score);
            }

            if(data.SCategory != null && data.SCategory.Length > 0)
            {
                foreach(var sCategory in data.SCategory)
                {
                    accommodations = accommodations.Where(a => a.AccommodationServiceInfos.Any(s => s.Name == sCategory));
                }
            }

            if (data.Regions != null && data.Regions.Length > 0)
            {
                accommodations = accommodations.Where(a => data.Regions.Contains(a.Region.Name));
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
