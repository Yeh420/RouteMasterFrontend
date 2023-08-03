using RouteMasterFrontend.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Comments_Accommodations
{
	public class Comments_AccommodationIndexVM
	{

		public int Id { get; set; }

		[Display(Name = "用戶帳號")]
		public string Account { get; set; }

		[Display(Name = "住宿名稱")]
		public string HotelName { get; set; }

		[Display(Name = "分數")]
		public int Score { get; set; }

		[Display(Name = "評論標題")]
		public string? Title { get; set; }

		[Display(Name = "優點")]
		public string? Pros { get; set; }

		[Display(Name = "缺點")]
		public string? Cons { get; set; }

		[Display(Name = "建立時間")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateDate { get; set; }

		public string Status { get; set; }
			
		public string? ReplyMessage { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ReplyDate { get; set; }

		public IEnumerable<Comments_AccommodationImage>? ImageList { get; set; }


	}
}
