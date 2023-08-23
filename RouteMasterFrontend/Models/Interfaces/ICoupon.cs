using RouteMasterFrontend.Models.Dto;
namespace RouteMasterFrontend.Models.Interfaces
{
    public interface ICoupon
    {
        Task<List<CouponsDto>> GetAllCouponsAsync();
    }

}
