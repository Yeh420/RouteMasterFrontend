namespace RouteMasterFrontend.Models.Dto
{
    public class Comments_AttractionCreateDTO
    {
        public int AttractionId { get; set; }
        public int Score { get; set; }
        public int? StayHours { get; set; }
        public int? Price { get; set; }
        public string? Content { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}
    