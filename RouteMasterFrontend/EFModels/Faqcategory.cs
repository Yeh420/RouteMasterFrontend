﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RouteMasterFrontend.EFModels
{
    public partial class Faqcategory
    {
        public Faqcategory()
        {
            Faqs = new HashSet<Faq>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Faq> Faqs { get; set; }
    }
}