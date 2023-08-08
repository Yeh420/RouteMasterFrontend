using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
	public class MemberLoginVM
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "帳號")]
		[Required]
		public string Account { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "密碼")]
		public string Password { get; set; }

		public string? image { get; set; }
	}
}
