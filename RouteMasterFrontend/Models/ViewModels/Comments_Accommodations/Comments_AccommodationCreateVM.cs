using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Comments_Accommodations
{
    public class Comments_AccommodationCreateVM
    {
        public int Id { get; set; }

        [Display(Name = "評論人")]
        public int MemberId { get; set; }

        [Display(Name = "住宿名稱")]
        public int AccommodationId { get; set; }

        [Display(Name = "分數")]
        [Required]
        public int Score { get; set; }

        [Display(Name = "標題")]
        [Required]
        public string? Title { get; set; }

        [Display(Name = "優點")]
        public string? Pros { get; set; }

        [Display(Name = "缺點")]
        public string? Cons { get; set; }

        [Display(Name = "上傳圖片")]
        public string? Images { get; set; }
    }
}
    