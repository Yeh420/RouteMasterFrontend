using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
    public class AccommodtaionsInfoDTO
    {
        public List<AccommodtaionsDTOItem> Items { get; set; }
        public int TotalPages { get; set; }
    }
}
