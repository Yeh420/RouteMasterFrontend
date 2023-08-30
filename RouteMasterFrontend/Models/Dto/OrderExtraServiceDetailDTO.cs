namespace RouteMasterFrontend.Models.Dto
{
	public class OrderExtraServiceDetailDTO
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ExtraServiceId { get; set; }
		public string ExtraServiceName { get; set; }
		
		public int ExtraServiceProductId { get; set; }
		public DateTime Date { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
        public string ImageUrl { get; set; }

    }
}