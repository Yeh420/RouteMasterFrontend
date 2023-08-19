using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
    public class MemberEditPasswordVM
    {
        public int id { get; set; }

        [Display(Name = "原始密碼")]
        [Required]
        [StringLength(20)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Display(Name = "新密碼")]
        [Required]
        [StringLength(20)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼")]
        [Required]
        [StringLength(20)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }

    }
}
