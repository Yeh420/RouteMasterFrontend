using System.ComponentModel.DataAnnotations;

namespace RouteMasterFrontend.Models.ViewModels.Carts
{
    public class Cart_ExtraServiceDetailsVM
    {
        
            
            public int Id { get; set; }
            public int CartId { get; set; }
            public int? ExtraServiceProductId { get; set; }
         
            public string? ExtraServiceName { get; set; }
        public string? ExtraServicePrice { get; set; }
        public int? Quantity { get; set; }
           
          
        
    }
}
