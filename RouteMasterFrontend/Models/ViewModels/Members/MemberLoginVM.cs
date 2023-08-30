using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
	public class MemberLoginVM
	{
        internal readonly bool RememberMe;

        [Key]
		public int Id { get; set; }

        [Required(ErrorMessage ="您尚未填寫帳號")]
        [Display(Name = "帳號")]
		public string Account { get; set; }

        [Required(ErrorMessage = "您尚未填寫密碼")]
        [DataType(DataType.Password)]
		[Display(Name = "密碼")]
		//[StringLength(20,ErrorMessage ="密碼至少6個字",MinimumLength =6)]
		public string Password { get; set; }

		public string? image { get; set; }
	}
}
