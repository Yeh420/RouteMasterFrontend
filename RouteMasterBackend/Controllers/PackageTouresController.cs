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
using static RouteMasterBackend.DTOs.PackageToursDto;


namespace RouteMasterBackend.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTouresController : ControllerBase
    {
        private readonly RouteMasterContext _context;


    
        public PackageTouresController(RouteMasterContext context)
        {
            _context = context;
        }


 



        [HttpGet]
        public async Task<IEnumerable<PackageToursDto>> GetAllPackageTours()
        {
            var packageInDb = _context.PackageTours
               .Include(x => x.Activities)
               .Include(x => x.Attractions)
               .Include(x => x.ExtraServices)
               .AsQueryable();


            var model = new List<PackageToursDto>();




            foreach (var item in packageInDb)
            {
                var data = new PackageToursDto();
                data.Id = item.Id;
                data.PackageActList = new List<actDtoInPackage>(); // 初始化集合
                data.PackageAttList = new List<attDtoInPackage>();
                data.PackageExtList = new List<extDtoInPackage>();


                var actsInItem = item.Activities;
                foreach (var act in actsInItem)
                {
                    var newActDtoInPackage = new actDtoInPackage
                    {
                        ActId = act.Id,
                        ActName = act.Name,
                    };
                    data.PackageActList.Add(newActDtoInPackage);
                }

                var attsInItem = item.Attractions;
                foreach (var att in attsInItem)
                {
                    var newAttDtoInPackage = new attDtoInPackage
                    {
                        AttId = att.Id,
                        AttName = att.Name,
                    };

                    data.PackageAttList.Add(newAttDtoInPackage);
                }


                var extsInItem = item.ExtraServices;
                foreach (var ext in extsInItem)
                {
                    var newExtDtoInPackage = new extDtoInPackage
                    {
                        ExtId = ext.Id,
                        ExtName = ext.Name,
                    };

                    data.PackageExtList.Add(newExtDtoInPackage);
                }
                model.Add(data);
            }




            return model;

        }
    }
}
