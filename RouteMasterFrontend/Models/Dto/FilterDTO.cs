namespace RouteMasterFrontend.Models.Dto
{
    public class FilterDTO
    {
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public IEnumerable<double?> Grades { get; set; }
        public IEnumerable<string> AcommodationCategories { get; set; }
        public IEnumerable<int> CommentSorce { get; set; }
        public List<ServiceDTO> ServiceInfoes { get; set; }
        public IEnumerable<string> Regions { get; set; }
    }
}
