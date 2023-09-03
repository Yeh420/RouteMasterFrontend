namespace RouteMasterFrontend.Models.Dto
{
	public class OrderActivityDetailDTO
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ActivityId { get; set; }
		public string ActivityName { get; set; }
		public int ActivityProductId { get; set; }
		
		public DateTime Date { get; set; }
		public TimeSpan StartTime { get; set; } 
		public TimeSpan EndTime { get; set; }  
		public decimal Price { get; set; }
		public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}