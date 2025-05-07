using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    // العلاقات مع الجداول الأخرى
    //public virtual ICollection<ContactU> ContactUs { get; set; } = new List<ContactU>();


    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PrintRequest> PrintRequests { get; set; } = new List<PrintRequest>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<ReviewsProduct> ReviewsProducts { get; set; }
    public virtual ICollection<Cart> Carts { get; set; } // علاقة مع عدة Carts

    public Address Address { get; set; }
    public PaymentMethod PaymentMethod { get; set; }  



    // يمكنك إضافة طريقة تحقق دور المستخدم
    public bool IsAdmin() => Role == "Admin";
    public bool IsCustomer() => Role == "Customer";
}
