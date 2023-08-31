namespace RouteMasterFrontend.Models.Dto
{
	public class OrderAccommodationDetailDTO
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int AccommodationId { get; set; }
		public string AccommodationName { get; set; }
		public int RoomProductId { get; set; }
		public string RoomType { get; set; }
		public string RoomName { get; set; }
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
		public int RoomPrice { get; set; }
		public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string Note { get; set; }
	}
}