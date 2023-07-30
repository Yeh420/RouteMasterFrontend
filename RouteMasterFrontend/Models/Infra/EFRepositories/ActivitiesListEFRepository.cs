using Microsoft.EntityFrameworkCore;
using RouteMasterFrontend.EFModels;
using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Infra.Criterias;
using RouteMasterFrontend.Models.Infra.ExtenSions;
using RouteMasterFrontend.Models.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RouteMasterFrontend.Models.Infra.EFRepositories
{
	public class ActivitiesListEFRepository : IActivityRepository
	{
		private readonly RouteMasterContext _context;	
        public ActivitiesListEFRepository(RouteMasterContext context)
        {
			_context = context;
        }
        public IEnumerable<ActivityListDto> Search(ActivityListCriteria criteria)
		{
		
			if (!string.IsNullOrEmpty(criteria.Name))
			{
				var query = _context.Activities.Where(x => x.Name.Contains(criteria.Name));

				return query.Include(a => a.ActivityCategory)
				.Include(a => a.Attraction)
				.Include(a => a.Region)
				.Select(x => x.ToListDto());
			}

			return _context.Activities
				.Include(a => a.ActivityCategory)
				.Include(a => a.Attraction)
				.Include(a => a.Region)
				.Select(x => x.ToListDto());
		}
	}
}
