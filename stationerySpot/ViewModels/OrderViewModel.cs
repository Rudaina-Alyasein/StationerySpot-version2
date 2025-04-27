using stationerySpot.Models;

namespace stationerySpot.ViewModels
{
    public class OrderViewModel
    {
        // من جدول Order
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        // من جدول User
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        // من جدول OrderDetail + Product
        //public List<ProductInfo> Products { get; set; }
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();

    }

    public class ProductInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
