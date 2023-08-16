namespace RouteMasterFrontend.Models.Dto
{
    public class Comments_AccommodationBriefDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }

        public int Score { get; set; }

        public string? Title { get; set; }

        public string? Pros { get; set; }

        public string? Cons { get; set; }

        public string CreateDate { get; set; }

        public int TotalThumbs { get; set; }
    }
}
