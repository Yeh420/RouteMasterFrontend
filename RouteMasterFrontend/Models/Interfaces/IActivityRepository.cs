using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Infra.Criterias;

namespace RouteMasterFrontend.Models.Interfaces
{
	public interface IActivityRepository
	{
		IEnumerable<ActivityListDto> Search(ActivityListCriteria criteria);
	}
}
