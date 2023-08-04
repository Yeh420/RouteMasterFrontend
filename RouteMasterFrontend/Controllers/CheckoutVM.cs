using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Controllers
{
    public class CheckoutVM
    {
        public Cart Cart { get; set; }
        public object AllowCheckout { get; set; }
        public string ErrorMessage { get; set; }
    }
}