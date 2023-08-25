using Humanizer;

namespace RouteMasterBackend.DTOs
{
    public class SchduleListDto
    {
        public int memberId { get; set; }
        public TableTBodyTrObj[]? tableTBodyTrObjs { get; set; }
        public DateTime createDate { get; set; }

    }

    public class TableTBodyTrObj
    {
        public string? itemName { get; set; }
        public string? itemPlaceOrItemDistance { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
    }
}
