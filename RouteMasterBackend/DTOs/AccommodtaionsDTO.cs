using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
	public class AccommodtaionsDTO
	{
        public List<AccommodtaionsDTOItem> Items { get; set; }
        public int TotalPages { get; set; }
	}
}
