namespace RouteMasterBackend.DTOs
{
    public class ActivityProductSelectCriteria
    {
        public int ActivityId { get; set; }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
    }

    public class ActivityProductInCalenderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }   
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
}
