using RouteMasterFrontend.EFModels;

namespace RouteMasterFrontend.Models.ViewModels.Carts
{
    public class CartVM
    {
        public CartVM()
        {
            Cart_AccommodationDetails = new HashSet<Cart_AccommodationDetail>();
            Cart_ActivitiesDetails = new HashSet<Cart_ActivitiesDetail>();
            Cart_ExtraServicesDetails = new HashSet<Cart_ExtraServicesDetail>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        //public int Total
        //{
        //    get
        //    {
        //        return Cart_AccommodationDetails.Sum(item => item.RoomProduct)
        //      + Cart_ActivitiesDetails.Sum(item => item.ActivityProduct.Price)
        //      + Cart_ExtraServicesDetails.Sum(item => item.ExtraServiceProduct.Price);
        //    }
        //}
        public bool AllowCheckout
        {
            get
            {
                return Cart_AccommodationDetails.Any()
                    || Cart_ActivitiesDetails.Any()
                    || Cart_ExtraServicesDetails.Any();
            }
        }
        public virtual Member Member { get; set; }
        public virtual ICollection<Cart_AccommodationDetail> Cart_AccommodationDetails { get; set; }
        public virtual ICollection<Cart_ActivitiesDetail> Cart_ActivitiesDetails { get; set; }
        public virtual ICollection<Cart_ExtraServicesDetail> Cart_ExtraServicesDetails { get; set; }

    }
}
