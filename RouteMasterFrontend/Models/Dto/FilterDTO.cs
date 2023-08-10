﻿namespace RouteMasterFrontend.Models.Dto
{
    public class FilterDTO
    {
        public IEnumerable<double?> Grades { get; set; }
        public IEnumerable<string> AcommodationCategories { get; set; }
        public IEnumerable<int> CommentSorce { get; set; }
        public IEnumerable<string> ServiceInfoCategories { get; set; }
        public IEnumerable<string> Regions { get; set; }
    }
}
