namespace RouteMasterFrontend.Models.Dto
{
    public class SystemMessageAjaxDTO
    {
        public int Id { get; set; }
        public string Category  { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
