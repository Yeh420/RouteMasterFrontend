namespace RouteMasterFrontend.Models.Dto
{
    public class Cart_AccommodationDetailDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string AccommodationName { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}