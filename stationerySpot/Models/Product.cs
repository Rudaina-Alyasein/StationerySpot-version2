using System.ComponentModel.DataAnnotations.Schema;

namespace stationerySpot.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ImagePath { get; set; }

    public int LibraryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Library Library { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    // إضافة الخاصية Reviews لربط التقييمات بالمنتج
    // العلاقة مع ReviewsProduct (مراجعات المنتج)
    public ICollection<ReviewsProduct> ReviewsProducts { get; set; }
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public int? OfferId { get; set; }  

    [ForeignKey("OfferId")]
    public virtual Offer? Offer { get; set; }

}
