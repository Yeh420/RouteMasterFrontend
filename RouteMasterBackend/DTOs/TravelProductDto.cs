namespace RouteMasterBackend.DTOs
{
    public class TravelProductDto
    {
        public int cartId { get; set; }
        public int[]? activityProductIds { get; set; }
        public int[]? extraServiceProductIds { get; set; }
        public RPDTO[]? roomProducts { get; set; }
    }
}
