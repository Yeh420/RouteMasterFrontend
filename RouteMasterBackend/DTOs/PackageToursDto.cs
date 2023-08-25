namespace RouteMasterBackend.DTOs
{
    public class PackageToursDto
    {
        public int Id { get; set; }
        public List<actDtoInPackage>? PackageActList { get; set; }
        public List<attDtoInPackage>? PackageAttList { get; set; }
        public List<extDtoInPackage>? PackageExtList { get; set; }
        public string? Description { get; set; }
        public class actDtoInPackage
        {
            public int  AttId { get; set; }
            public int ActId { get; set; }
            public string? ActName { get; set; }
            public string? ActImage { get; set; }
            public string? Description { get; set; }
        }

        public class attDtoInPackage
        {
            public int AttId { get; set; }
            public string? AttName { get; set; }
            public string? AttImage { get; set; }
        }

        public class extDtoInPackage
        {
            public int AttId { get; set; }
            public int ExtId { get; set; }
            public string? ExtName { get; set; }
            public string? ExtImage { get; set; }

            public string? Description { get; set; }
        }
    }
}
