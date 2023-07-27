using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Infra.Criterias;
using RouteMasterFrontend.Models.Interfaces;

namespace RouteMasterFrontend.Models.Services
{
    public class ActivityService
    {
        private IActivityRepository _repo;

        public ActivityService(IActivityRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<ActivityListDto> Search(ActivityListCriteria criteria)
        {
            return _repo.Search(criteria);  
        }
    }
}
