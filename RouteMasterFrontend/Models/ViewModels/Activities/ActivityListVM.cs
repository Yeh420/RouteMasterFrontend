using System.ComponentModel.DataAnnotations;

namespace RouteMasterFrontend.Models.ViewModels.Activities
{
    public class ActivityListVM
    {
		public int Id { get; set; }

		[Display(Name = "分類")]
		public string? ActivityCategoryName { get; set; }
		[Display(Name = "縣市")]
		public string? RegionName { get; set; }
		[Display(Name = "活動名稱")]
		public string? Name { get; set; }

		[Display(Name = "舉辦景點")]
		public string? AttractionName { get; set; }

		[Display(Name = "活動介紹")]
		public string? Description { get; set; }

		[Display(Name = "當前狀態")]
		public bool Status { get; set; }

		[Display(Name="圖片")]
		public string? Image { get; set; }
	}
}
