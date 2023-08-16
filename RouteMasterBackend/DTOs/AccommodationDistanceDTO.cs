using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
    public class AccommodationDistanceDTO
    {
        public int? Id { get; set; }
        public string? ACategory { get; set; }
        public string? Name { get; set; }
        public double? Grade { get; set; }
        public string? Address { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string? Image { get; set; }
        public double? Distance { get; set; }

        public IEnumerable<Room> Rooms { get; set; }
        //public IEnumerable<RoomProduct> RoomProducts { get; set; }
    }
}
