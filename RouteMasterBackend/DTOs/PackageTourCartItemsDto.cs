namespace RouteMasterBackend.DTOs
{
    public class PackageTourCartItemsDto
    {
        public int cartId { get; set; }
        public List<ExtPackageTourProductCartItem>? selectedExtProductIdsWithQuantity { get; set; }

        public List<ActPackageTourProductCartItem>? selectedActProductIdsWithQuantity { get; set; }

    }
    public class ExtPackageTourProductCartItem
    {
        public int id  { get; set; }
        public int attId { get; set; }
        public int quantity { get; set; }
    }
    public class ActPackageTourProductCartItem
    {
        public int id { get; set; }
        public int attId { get; set; }
        public int quantity { get; set; }
    }



}
