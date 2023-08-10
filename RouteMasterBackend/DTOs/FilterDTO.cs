namespace RouteMasterBackend.DTOs
{
    public class FilterDTO
    {
        public List<int> Grades { get; set; }
        public List<string> AcommodationCategories { get; set; }
        public List<int> CommentScore{ get; set; }
        public List<string> ServiceInfoCategories { get; set; }
        public List<int> Regions{ get; set; }
    }
}
