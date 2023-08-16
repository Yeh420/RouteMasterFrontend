using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RouteMasterBackend.DTOs
{
    public class ExtraServiceProductSelectCriteria
    {
        public int ExtraServiceId  { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }    
    }


    public class ExtraServiceProductInCalenderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
