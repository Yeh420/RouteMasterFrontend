using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Models.Dto
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<AccommodationServiceInfo>? AccommodationServiceInfos { get; set; }
    }
}
