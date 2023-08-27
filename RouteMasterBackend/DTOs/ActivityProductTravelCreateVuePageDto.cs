namespace RouteMasterBackend.DTOs
{
    public class ActivityProductTravelCreateVuePageDto
    {
        public int ActivityProductId { get; set; }

        public string? ActivityName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }  

    }
}
