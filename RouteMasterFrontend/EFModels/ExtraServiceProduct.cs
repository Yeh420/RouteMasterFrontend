﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RouteMasterFrontend.EFModels
{
    public partial class ExtraServiceProduct
    {
        public ExtraServiceProduct()
        {
            Cart_ExtraServicesDetails = new HashSet<Cart_ExtraServicesDetail>();
            OrderExtraServicesDetails = new HashSet<OrderExtraServicesDetail>();
            TravelPlans = new HashSet<TravelPlan>();
        }

        public int Id { get; set; }
        public int ExtraServiceId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public virtual ExtraService ExtraService { get; set; }
        public virtual ICollection<Cart_ExtraServicesDetail> Cart_ExtraServicesDetails { get; set; }
        public virtual ICollection<OrderExtraServicesDetail> OrderExtraServicesDetails { get; set; }

        public virtual ICollection<TravelPlan> TravelPlans { get; set; }
    }
}