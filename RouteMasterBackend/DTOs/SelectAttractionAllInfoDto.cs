namespace RouteMasterBackend.DTOs
{
    public class SelectAttractionAllInfoDto
    {
        public int AttractionId { get; set; }
        public string? AttractionName { get; set; }
        public int? StayHours { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }

        public List<ExtInAtt>? ExtListInAtt { get; set; }

        public List<ActInAtt>? ActListInAtt { get; set; }

    }
    public class ExtInAtt
    {
        public int ExtId { get; set; }
        public string?  ExtName  { get; set; }       
    }

    public class ActInAtt
    {
        public int ActId { get; set; }
        public string? ActName { get; set; }
    }
}
