﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RouteMasterBackend.Models
{
    public partial class FavoriteAttraction
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int AttractionId { get; set; }

        public virtual Attraction Attraction { get; set; }
        public virtual Member Member { get; set; }
    }
}