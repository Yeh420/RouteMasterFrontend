namespace RouteMasterBackend.DTOs
{
    public class FilterData
    {
        public string[]? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public double?[]? Grades { get; set; }
        public string?[]? ACategory { get; set; }
        public int? score { get; set; }
        public string?[]? SCategory { get; set; }
        public string?[]? Regions { get; set; }
    }
}
