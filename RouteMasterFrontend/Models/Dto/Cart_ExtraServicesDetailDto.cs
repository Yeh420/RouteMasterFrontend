namespace RouteMasterFrontend.Models.Dto
{
    public class Cart_ExtraServicesDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}