namespace RouteMasterFrontend.Models.Dto
{
    public class Comments_AccommodationCreateDTO
    {
        public int AccommodationId { get; set; }
        public int Score { get; set; }
        public string? Title { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}
