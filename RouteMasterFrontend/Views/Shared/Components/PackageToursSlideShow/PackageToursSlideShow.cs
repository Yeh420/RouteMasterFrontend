using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using RouteMasterFrontend.EFModels;
using System;
using static RouteMasterFrontend.Views.Shared.Components.PackageToursSlideShow.PackageToursSlideShowViewComponent.PaackageTourSlideShowDto;

namespace RouteMasterFrontend.Views.Shared.Components.PackageToursSlideShow
{
    public class PackageToursSlideShowViewComponent:ViewComponent
    {
        private readonly RouteMasterContext _context;
        public PackageToursSlideShowViewComponent(RouteMasterContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var packageInDb = _context.PackageTours
                .Include(x => x.Activities)
                .Include(x => x.Attractions)
                .Include(x => x.ExtraServices)
                .AsQueryable();


            var model = new List<PaackageTourSlideShowDto>();




            foreach (var item in packageInDb)
            {
                var data = new PaackageTourSlideShowDto();
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



            return View("_PackageToursPartial", model);
        }
        public class PaackageTourSlideShowDto
        {
            public int Id { get; set; }
            public List<actDtoInPackage>? PackageActList { get; set; }
            public List<attDtoInPackage>? PackageAttList { get; set; }
            public List<extDtoInPackage>? PackageExtList { get; set; }
            public class actDtoInPackage
            {
                public int ActId { get; set; }
                public string? ActName { get; set; }
            }

            public class attDtoInPackage
            {
                public int AttId { get; set; }
                public string? AttName { get; set; }
            }

            public class extDtoInPackage
            {
                public int ExtId { get; set; }
                public string? ExtName { get; set; }
            }
        }
    }
}
