namespace RouteMasterFrontend.Models.Dto
{
    public class Cart_ActivitiesDetailDto
    {
        public int Id { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}