using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
    public class MemberResetPasswordVM
    {
        [Display(Name = "新密碼")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "{0} 必填")]
        [StringLength(50)]
        [Compare(nameof(Password),ErrorMessage = "與密碼不相同")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        public string Account { get; set; }

        public string confirmCode { get; set; }
    }
}
