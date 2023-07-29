﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RouteMasterFrontend.EFModels
{
    public partial class Member
    {
        public Member()
        {
            Carts = new HashSet<Cart>();
            CommentAccommodationLikes = new HashSet<CommentAccommodationLike>();
            CommentsAccommodations = new HashSet<CommentsAccommodation>();
            CommentsAttractions = new HashSet<CommentsAttraction>();
            FavoriteAttractions = new HashSet<FavoriteAttraction>();
            MemberImages = new HashSet<MemberImage>();
            Orders = new HashSet<Order>();
            Schedules = new HashSet<Schedule>();
            SystemMessages = new HashSet<SystemMessage>();
            TravelPlans = new HashSet<TravelPlan>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Account { get; set; }
        public string EncryptedPassword { get; set; }
        public string Email { get; set; }
        public string CellPhoneNumber { get; set; }
        public string Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmCode { get; set; }
        public string GoogleAccessCode { get; set; }
        public string FaceBookAccessCode { get; set; }
        public string LineAccessCode { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime? LoginTime { get; set; }
        public bool IsSuscribe { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<CommentAccommodationLike> CommentAccommodationLikes { get; set; }
        public virtual ICollection<CommentsAccommodation> CommentsAccommodations { get; set; }
        public virtual ICollection<CommentsAttraction> CommentsAttractions { get; set; }
        public virtual ICollection<FavoriteAttraction> FavoriteAttractions { get; set; }
        public virtual ICollection<MemberImage> MemberImages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<SystemMessage> SystemMessages { get; set; }
        public virtual ICollection<TravelPlan> TravelPlans { get; set; }
    }
}