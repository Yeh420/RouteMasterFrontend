namespace RouteMasterBackend.DTOs
{
    public class UpdateExtQuantityDto
    {
        public int CartId { get; set; }
        public int ExtraServiceProductId { get; set; }
        public int Quantity { get; set; }
    }
}
