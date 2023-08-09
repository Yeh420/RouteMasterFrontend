namespace RouteMasterFrontend.Models.Dto
{
    public class AttractionDetailDto
    {
        public int Id { get; set; }
        public string AttractionCategory { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public double Score { get; set; }
        public int ScoreCount { get; set; }
        public double Hours { get; set; }
        public int HoursCount { get; set; }
        public int Price { get; set; }
        public int PriceCount { get; set; }
        public int Clicks { get; set; }
        public string Website { get; set; }
    }
}
