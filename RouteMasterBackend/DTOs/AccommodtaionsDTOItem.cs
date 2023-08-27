using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
	public class AccommodtaionsDTOItem
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Grade { get; set; }
        public string Address { get; set; }
        public string? Image { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string Score { get; set; }
        public int Comment { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<AccommodationServiceInfo> Services { get; set; }
    }
}
