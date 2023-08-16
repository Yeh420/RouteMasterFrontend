namespace RouteMasterFrontend.Models.Dto
{
    public class FAQIndexDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Helpful { get; set; }
        public string Image { get; set; }
    }
}
