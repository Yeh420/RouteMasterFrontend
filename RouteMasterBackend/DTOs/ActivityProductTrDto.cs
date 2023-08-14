namespace RouteMasterBackend.DTOs
{
    public class ActivityProductTrDto
    {
        public int Id { get; set; }
        public string? ActivityName { get; set; }
        public string? AttractionName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
