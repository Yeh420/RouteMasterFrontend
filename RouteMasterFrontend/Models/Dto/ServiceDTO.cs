using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Models.Dto
{
    public class ServiceDTO
    {
        public string Name { get; set; }
        public IEnumerable<string> Infos { get; set; }
    }
}
