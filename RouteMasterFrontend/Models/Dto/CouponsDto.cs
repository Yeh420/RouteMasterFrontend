namespace RouteMasterFrontend.Models.Dto
{
    public class CouponsDto
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public int Discount { get; set; }
        public DateTime StartDate { get; set; }

        public string StartDateText
        {
            get
            {
                return StartDate.ToString("yyyy/MM/dd");
            }
        }
        public DateTime EndDate { get; set; }

        public string EndDateText
        {
            get
            {
                return EndDate.ToString("yyyy/MM/dd");
            }
        }
        public bool IsActive { get; set; }
        public bool? Valuable { get; set; }

        public bool Selected { get; set; } = false;
    }
}
