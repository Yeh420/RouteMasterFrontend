namespace RouteMasterBackend.DTOs
{
    public class UpdateActQuantityDto
    {
        public int CartId { get; set; }
        public int ActivityProductId { get; set; }
        public int Quantity { get; set; }
    }
}
