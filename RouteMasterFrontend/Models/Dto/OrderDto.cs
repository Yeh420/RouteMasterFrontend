namespace RouteMasterFrontend.Models.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int PaymentMethodId { get; set; }
        public int PaymentStatusId { get; set; }
        public string? PaymentStatusName { get; set; }
        public string? PaymentMethodName { get; set; }
        public int OrderHandleStatusId { get; set; }
        public int? CouponsId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Total { get; set; }
		public List<OrderExtraServiceDetailDTO>? ExtraServiceDetails { get; set; }
		public List<OrderActivityDetailDTO>? ActivityDetails { get; set; }
		public List<OrderAccommodationDetailDTO>? AccommodationDetails { get; set; }
     
	}
}
