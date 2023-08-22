using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
    public class AccommodationDistanceDTO
    {
        public int? Id { get; set; }
        public string? ACategory { get; set; }
        public string? Name { get; set; }
        public double? Grade { get; set; }
        public string? Score { get; set; }
        public string? Address { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }  
        public string? Image { get; set; }
        public double? Distance { get; set; }
        public int Comment { get; set; }
        public IEnumerable<AccommodationDistanceRDTO> Rooms { get; set; }
    }
}
