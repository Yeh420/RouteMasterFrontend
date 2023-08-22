namespace RouteMasterFrontend.Models.Dto
{
    public class MemberEditDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        //public DateTime CreateDate { get; set; }
        public bool IsSuscribe { get; set; }
    }
}
