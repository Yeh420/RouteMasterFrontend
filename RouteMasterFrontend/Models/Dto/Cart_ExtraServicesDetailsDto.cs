namespace RouteMasterFrontend.Models.Dto
{
    public class Cart_ExtraServicesDetailsDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ExtraServiceProductId { get; set; }
        public List<string> ExtraServiceImages { get; set; }
        public string ExtraServiceName { get; set; }

    }
}
