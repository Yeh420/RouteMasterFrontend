using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
	public class AccommodtaionsDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Grade { get; set; }
        public string Address { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string Score { get; set; }
        public int Comments { get; set; }
        public string Image { get; set; }
        public IEnumerable<AccommodationImage> Images { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<AccommodationServiceInfo> Services { get; set; }
    }
}
