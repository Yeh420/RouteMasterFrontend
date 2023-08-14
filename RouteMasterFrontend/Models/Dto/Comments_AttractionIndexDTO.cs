﻿namespace RouteMasterFrontend.Models.Dto
{
    public class Comments_AttractionIndexDTO
    {
        public int Id { get; set; }
        public string Account { get; set; } 
        public string AttractionName  { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public int StayHours { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsHidden { get; set; }

        public IEnumerable<string>? ImageList { get; set; }
    }
}