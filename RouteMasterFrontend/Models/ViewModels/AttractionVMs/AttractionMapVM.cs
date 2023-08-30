namespace RouteMasterFrontend.Models.ViewModels.AttractionVMs
{
    public class AttractionMapVM
    {
        public int Id { get; set; }
        public string AttractionCategory { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string Image { get; set; }
        public List<string> Tags { get; set; }
        public double Score { get; set; }
        public int ScoreCount { get; set; }
        public double Hours { get; set; }
        public int HoursCount { get; set; }
        public int Price { get; set; }
        public int PriceCount { get; set; }
        public int Clicks { get; set; }

        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
       
    }
}
