using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
    public class AttractionInfoDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? StayHours { get; set; }
        public List<ActivityProduct>? ActivityProducts { get; set; }
        public List<ExtraServiceProduct>? ExtraServiceProducts { get; set; }

    }
}
