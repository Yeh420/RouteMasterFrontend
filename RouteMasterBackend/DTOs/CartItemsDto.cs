namespace RouteMasterBackend.DTOs
{
    public class CartItemsDto
    {
        public int cartId { get; set; }
        public List<ActivityCartItem>? ActivityCartItems { get; set; }
        public List<ExtraServiceCartItem>? ExtraServiceCartItems { get; set; }
        public List<AccommodationCartItem>? AccommodationCartItems { get; set; }

    }
    public class ActivityCartItem
    {
        public string? imgUrl { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
    public class ExtraServiceCartItem
    {
        public string? Name { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }

    public class AccommodationCartItem
    {
        public string imgUrl { get; set; }
        public string AccommodationName { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
