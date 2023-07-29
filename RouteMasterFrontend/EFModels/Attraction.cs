﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RouteMasterFrontend.EFModels
{
    public partial class Attraction
    {
        public Attraction()
        {
            Activities = new HashSet<Activity>();
            AttractionClicks = new HashSet<AttractionClick>();
            AttractionImages = new HashSet<AttractionImage>();
            CommentsAttractions = new HashSet<CommentsAttraction>();
            ExtraServices = new HashSet<ExtraService>();
            FavoriteAttractions = new HashSet<FavoriteAttraction>();
            PackageTours = new HashSet<PackageTour>();
            Tags = new HashSet<AttractionTag>();
            TravelPlans = new HashSet<TravelPlan>();
        }

        public int Id { get; set; }
        public int AttractionCategoryId { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public int TownId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }

        public virtual AttractionCategory AttractionCategory { get; set; }
        public virtual Region Region { get; set; }
        public virtual Town Town { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<AttractionClick> AttractionClicks { get; set; }
        public virtual ICollection<AttractionImage> AttractionImages { get; set; }
        public virtual ICollection<CommentsAttraction> CommentsAttractions { get; set; }
        public virtual ICollection<ExtraService> ExtraServices { get; set; }
        public virtual ICollection<FavoriteAttraction> FavoriteAttractions { get; set; }

        public virtual ICollection<PackageTour> PackageTours { get; set; }
        public virtual ICollection<AttractionTag> Tags { get; set; }
        public virtual ICollection<TravelPlan> TravelPlans { get; set; }
    }
}