using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Models.Interfaces
{
    public interface ICartService
    {
        int CalculateCartTotal(Cart cart);
    }
}
