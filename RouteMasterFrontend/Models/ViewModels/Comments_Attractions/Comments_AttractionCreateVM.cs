using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Comments_Attractions
{
    public class Comments_AttractionCreateVM
    {
        public int Id { get; set; }
        public int Score { get; set; }

        [Display(Name = "評論內文")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "停留時間(小時)")]
        public int? StayHours { get; set; }

        [Display(Name = "花費(元)")]
        public int? Price { get; set; }

        [Display(Name = "上傳圖片")]
        public string? Images { get; set; }

    }
}
