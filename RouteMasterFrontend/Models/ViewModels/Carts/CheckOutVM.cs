using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Carts
{
    public class CheckOutVM
    {
        [Display(Name = "收件人")]
        [Required]
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [Display(Name = "地址")]
        [Required]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Display(Name = "電子信箱")]
        [Required]
        [MaxLength(10)]
        public string? Email { get; set; }
    }
}
