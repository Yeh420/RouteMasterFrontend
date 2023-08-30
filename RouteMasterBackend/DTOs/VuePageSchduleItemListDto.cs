namespace RouteMasterBackend.DTOs
{
	public class VuePageSchduleItemListDto
	{
        public int memberId { get; set; }
        public TravelVuePageSchduleObj[]? VuePageSchduleObjs { get; set; }
        public DateTime createDate { get; set; } 
    }

    public class TravelVuePageSchduleObj
    {
        public string? title { get; set; }
        public string? start { get; set; }
        public string? end { get; set; }


    }


}
