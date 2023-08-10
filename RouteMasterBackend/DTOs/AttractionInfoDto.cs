using RouteMasterBackend.Models;

namespace RouteMasterBackend.DTOs
{
    public class AttractionInfoDto
    {
        public int Id { get; set; }
        public string? AttractionName { get; set; }
        public int? StayHours { get; set; }
        public List<ActivityProductShowOnTravelPlan>? ActivityProducts { get; set; }
        public List<ExtraServiceProductShowOnTravelPlan>? ExtraServiceProducts { get; set; }

    }


   public class ActivityProductShowOnTravelPlan
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }


    public class ExtraServiceProductShowOnTravelPlan
    {
        public int Id { get; set; }
        public int ExtraServiceId { get; set; }
        public string? ExtraServiceName { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
}
