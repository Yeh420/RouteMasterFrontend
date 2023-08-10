namespace RouteMasterBackend.DTOs
{
    public class FilterData
    {
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public double?[]? Grades { get; set; }
    }
}
