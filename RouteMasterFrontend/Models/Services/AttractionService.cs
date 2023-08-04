using RouteMasterFrontend.Models.Dto;
using RouteMasterFrontend.Models.Interfaces;

namespace RouteMasterFrontend.Models.Services
{
    public class AttractionService
    {
        private IAttractionRepository _repo;

        public AttractionService(IAttractionRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<AttractionIndexDto> Search()
        {
            return _repo.Search();
        }

        public IEnumerable<AttractionIndexDto> GetTopTen()
        {
            return _repo.GetTopTen();
        }
    }
}
