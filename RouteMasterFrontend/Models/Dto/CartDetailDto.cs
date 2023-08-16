namespace RouteMasterFrontend.Models.Dto
{
    public class CartDetailDto
    {
        public List<Cart_ExtraServicesDetailDto>? ExtraServices { get; set; }
        public List<Cart_AccommodationDetailDto>? Accommodations { get; set; }
        public List<Cart_ActivitiesDetailDto>? Activities { get; set; }
    }
}
