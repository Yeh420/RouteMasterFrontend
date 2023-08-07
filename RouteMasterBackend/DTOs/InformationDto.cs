namespace RouteMasterBackend.DTOs
{
    public class InformationDto
    {
        public Location? Start { get; set; }
        public Location? End { get; set; }
        public Distance? Distance { get; set; }
        public Duration? Duration { get; set; }
    }

   public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Distance
    {
        public string? Text { get; set; }
        public int? Value { get; set; }
    }

    public class Duration
    {
        public string? Text { get; set; }
        public int? Value { get; set; }  
    }
}
