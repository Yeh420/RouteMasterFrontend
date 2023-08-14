using Microsoft.EntityFrameworkCore.Metadata;

namespace RouteMasterBackend.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int PaymentStatusId { get; set; }
    }
}
