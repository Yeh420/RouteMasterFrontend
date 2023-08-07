using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RouteMasterFrontend.Models.ViewModels.Members
{
    public class MemberDetailVM
    {
        public int Id { get; set; }

        [Display(Name = "名字")]
        public string FirstName { get; set; }

        [Display(Name = "姓氏")]
        public string LastName { get; set; }

        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Display(Name = "註冊信箱")]
        public string Email { get; set; }

        [Display(Name = "連絡電話")]
        public string CellPhoneNumber { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "性別")]
        public bool? Gender { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "註冊日期")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "大頭貼")]
        public string Image { get; set; }

        [Display(Name = "完成開通")]
        public bool IsConfirmed { get; set; }


        [Display(Name = "上次登入時間")]
        public DateTime? LoginTime { get; set; }

        [Display(Name = "廣告訂閱")]
        public bool IsSuscribe { get; set; }

    }
}
