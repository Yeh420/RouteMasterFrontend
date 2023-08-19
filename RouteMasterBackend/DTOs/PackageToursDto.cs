namespace RouteMasterBackend.DTOs
{
    public class PackageToursDto
    {
        public int Id { get; set; }
        public List<actDtoInPackage>? PackageActList { get; set; }
        public List<attDtoInPackage>? PackageAttList { get; set; }
        public List<extDtoInPackage>? PackageExtList { get; set; }
        public class actDtoInPackage
        {
            public int ActId { get; set; }
            public string? ActName { get; set; }
        }

        public class attDtoInPackage
        {
            public int AttId { get; set; }
            public string? AttName { get; set; }
        }

        public class extDtoInPackage
        {
            public int ExtId { get; set; }
            public string? ExtName { get; set; }
        }
    }
}
