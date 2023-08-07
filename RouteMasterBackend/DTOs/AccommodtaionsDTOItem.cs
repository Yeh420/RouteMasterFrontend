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
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public string Website { get; set; }
        public string IndustryEmail { get; set; }
        public string PhoneNumber { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public IEnumerable<AccommodationImage> Images { get; set; }
        public IEnumerable<CommentsAccommodation> Comments { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<AccommodationServiceInfo> Services { get; set; }
    }
}
