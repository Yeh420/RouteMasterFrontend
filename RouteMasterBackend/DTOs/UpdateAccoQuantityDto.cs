namespace RouteMasterBackend.DTOs
{
    public class UpdateAccoQuantityDto
    {
        public int CartId { get; set; }
        public int RoomProductId { get; set; }
        public int Quantity { get; set; }
    }
}
