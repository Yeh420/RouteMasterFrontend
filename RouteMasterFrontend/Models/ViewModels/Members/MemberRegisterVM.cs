using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
	public class MemberRegisterVM
	{
		public int Id { get; set; }

		[Display(Name = "名")]
        [Required(ErrorMessage = "必填")]
        [StringLength(50)]
		public string? FirstName { get; set; }


		[Display(Name = "姓")]
		[Required(ErrorMessage = "必填")]
		[StringLength(50)]
		public string? LastName { get; set; }


		[Display(Name = "帳號")]
		[Required(ErrorMessage = "必填")]
		[StringLength(30)]
		[Remote(action: "CheckRepeatAccount", controller:"Members", AdditionalFields = nameof(Account))]
		public string? Account { get; set; }


		[Display(Name = "密碼")]
        [Required(ErrorMessage = "必填")]       
		[DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "密碼必須介於6~10個數字或是字母")]
        public string? Password { get; set; }

		[Display(Name = "密碼確認")]
		[Compare("Password", ErrorMessage = "必需與您輸入的'密碼'相同")]		
		[DataType(DataType.Password)]
		
		public string? ConfirmPassword { get; set; }



		[Display(Name = "電子信箱")]
        [Required(ErrorMessage = "必填")]
        //[StringLength(255)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote(action: "CheckRepeatEmail", controller: "Members", AdditionalFields = nameof(Email))]
        public string? Email { get; set; }


		[Display(Name = "電話號碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "無效電話號碼")]
		public string? CellPhoneNumber { get; set; }


		[Display(Name = "地址")]
		[StringLength(255)]
		[Required]
		public string? Address { get; set; }


		[Display(Name = "性別")]
        [Required(ErrorMessage = "必填")]
        public bool Gender { get; set; }


		//[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		//[Display(Name = "生日")]
		//public DateTime Birthday { get; set; }



		[Display(Name = "大頭貼")]

		public string? Image { get; set; }


		//[Display(Name ="廣告訂閱")]
		//public bool IsSuscribe { get; set; }
	}
}
