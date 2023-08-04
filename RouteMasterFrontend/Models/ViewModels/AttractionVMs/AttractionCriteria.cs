namespace RouteMasterFrontend.Models.ViewModels.AttractionVMs
{
    public class AttractionCriteria
    {
        public string Keyword { get; set; }
        public List<string> category { set; get; }
        public List<string> tag { set; get; }
        public List<string> region { set; get; }
        public string order { set; get; }
    }
}
