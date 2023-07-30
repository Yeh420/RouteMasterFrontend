namespace RouteMasterFrontend.Models.Infra.Criterias
{
	public class ActivityListCriteria
	{
		public string? Name { get; set; }
		public int? ActivityCategoryId { get; set; }
		public int? AttractionId { get; set; }
		public int? RegionId { get; set; }
		public bool ShowAvailableOnly { get; set; }

	}
}
