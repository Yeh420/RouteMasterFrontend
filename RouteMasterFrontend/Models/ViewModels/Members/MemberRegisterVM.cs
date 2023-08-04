using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
	public class MemberRegisterVM
	{
		public int Id { get; set; }

		[Display(Name = "名")]
		[Required]
		[StringLength(50)]
		public string? FirstName { get; set; }


		[Display(Name = "姓")]
		[Required]
		[StringLength(50)]
		public string? LastName { get; set; }


		[Display(Name = "帳號")]
		[Required]
		[StringLength(30)]
		public string? Account { get; set; }


		[Display(Name = "密碼")]
		[Required]
		[StringLength(20)]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[Display(Name = "密碼確認")]
		[Compare("Password", ErrorMessage = "必需與您輸入的'密碼'相同")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }



		[Display(Name = "電子信箱")]
		[Required]
		//[StringLength(255)]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string? Email { get; set; }


		[Display(Name = "電話號碼")]
		[Required]
		[StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid Phone Number")]
		public string? CellPhoneNumber { get; set; }


		[Display(Name = "地址")]
		[StringLength(255)]
		public string? Address { get; set; }


		[Display(Name = "性別")]
		public bool Gender { get; set; }


		//[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		//[Display(Name = "生日")]
		//public DateTime Birthday { get; set; }



		[Display(Name = "大頭貼")]
		
		public string? Image { get; set; }



		public bool IsSuscribe { get; set; }
	}
}
