namespace RouteMasterBackend.DTOs
{
    public class TravelProductDto
    {
        public int cartId { get; set; }
        public ActProductIdAndQuantiyFromTravlePlan[]? activityProductIds { get; set; }
        public ExProductIdAndQuantiyFromTravlePlan[]? extraServiceProductIds { get; set; }
        public RPDTO[]? roomProducts { get; set; }
    }

    public class ActProductIdAndQuantiyFromTravlePlan
    {
        public int actProductId { get; set; }
        public int quantity { get; set; }
    }


    public class ExProductIdAndQuantiyFromTravlePlan
    {
        public int extProductId { get; set; }
        public int quantity { get; set; }
    }
}
