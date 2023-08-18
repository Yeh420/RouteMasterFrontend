namespace RouteMasterFrontend.Models.Dto
{
    public class HistoryOrderDTO
    {
        
        public int MemberId { get; set; }
        public string Account { get; set; }
        public int PaymentStatusId { get; set; }
        //public int OrderHandleStatusId { get; set; }
        //public int CouponsId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Total { get; set; }

        
    }
    public class Abc
    {
        public int pagecase { get; set; }
    }
}
