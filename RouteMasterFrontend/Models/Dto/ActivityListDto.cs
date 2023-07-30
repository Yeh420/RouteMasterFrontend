namespace RouteMasterFrontend.Models.Dto
{
	public class ActivityListDto
	{
		public int Id { get; set; }
        public string? RegionName { get; set; }
        public string? ActivityCategoryName { get; set; }   
        public string? Name { get; set; }
        public string? AttractionName { get; set; }
        public string? Description { get; set; }
		public bool Status { get; set; }
		public string? Image { get; set; }
	}
}
