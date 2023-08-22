namespace RouteMasterBackend.DTOs
{
    public class ExtraServiceVuePageIndexDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public bool Status { get; set; }
        public string? Description { get; set; }

        public string? RegionName { get; set; }

        public string? AttractionName { get; set; }

    }

    public class ExtraServiceCriteria
    {
        public string? Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }

    public class ExtraServicePagingVueDto
    {
        public int TotalPage { get; set; }

        public List<ExtraServiceVuePageIndexDto>? ExtraServiceVuePageDtoes { get; set; }
    }


}

