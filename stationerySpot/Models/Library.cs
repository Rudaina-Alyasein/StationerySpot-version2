using System;
using System.Collections.Generic;

namespace stationerySpot.Models
{
    public partial class Library
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;


        public string Location { get; set; } = null!;

        public string? Description { get; set; }

        public string? Phone { get; set; }  // العمود الجديد

        public string? LogoPath { get; set; }  // العمود الجديد

        public string? WorkingHours { get; set; }  // العمود الجديد

        public string? WebsiteUrl { get; set; }  // URL → Url
        public string? Status { get; set; } = "New";


        public string? City { get; set; }  // إضافة حقل المدينة الجديد

        public DateTime? CreatedAt { get; set; }

        // العلاقة مع حساب المكتبة
        //public virtual ICollection<ContactU> ContactUs { get; set; } = new List<ContactU>();

        public virtual ICollection<LibraryAccount> LibraryAccounts { get; set; } = new List<LibraryAccount>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        //public virtual ICollection<ContactU> ReceivedMessages { get; set; } = new List<ContactU>();


        public virtual ICollection<PrintRequest> PrintRequests { get; set; } = new List<PrintRequest>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
        // 💡 العلاقة: مكتبة فيها عدة مشاريع
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
