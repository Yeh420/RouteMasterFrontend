namespace RouteMasterBackend.DTOs
{
    public class RoomProductsDTO
    {
        public List<RoomProductDTOItem> Items { get; set; }
        public List<string> DisableDate { get; set; }
    }
}