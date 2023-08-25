namespace RouteMasterFrontend.Models.Dto
{
    public class MemberEditDTO
    {
        public string account { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string cellPhoneNumber { get; set; }
        public string address { get; set; }
        public bool? gender { get; set; }
        public DateTime? birthday { get; set; }
        //public DateTime CreateDate { get; set; }
        public bool isSuscribe { get; set; }
    }
}
