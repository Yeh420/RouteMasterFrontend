namespace RouteMasterFrontend.Models.ViewModels.AttractionVMs
{
    public class AttractionForDistsnceVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? PosX { get; set; }
        public decimal? PosY { get; set; }

        public decimal? Distance { get; set;}
    }
}
