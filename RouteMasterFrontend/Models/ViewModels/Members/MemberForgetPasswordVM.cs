using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
    public class MemberForgetPasswordVM
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(30)]
        public string Account { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "{0} 格式有誤")]
        public string Email { get; set; }
    }
}
