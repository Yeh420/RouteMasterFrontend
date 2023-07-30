
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.ViewModels.Activities;

namespace RouteMasterFrontend.Models.Infra.ExtenSions
{
	public static class ActivityExts
	{
		public static ActivityListDto ToListDto(this Activity entity)
		{
			return new ActivityListDto
			{
				Id = entity.Id,
				RegionName = entity.Region.Name,
				ActivityCategoryName=entity.ActivityCategory.Name,
				Name = entity.Name,
				AttractionName=entity.Attraction.Name,	
				Description = entity.Description,
				Status = entity.Status,
				Image = entity.Image
			};
		}
		public static ActivityListVM ToListVM(this ActivityListDto dto)
		{
			return new ActivityListVM
			{
				Id=dto.Id,
				RegionName=dto.RegionName,
				ActivityCategoryName=dto.ActivityCategoryName,
				Name=dto.Name,
				AttractionName=dto.AttractionName,
				Description=dto.Description,
				Status = dto.Status,
				Image = dto.Image	
			};
		}


	}
}
