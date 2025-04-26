namespace stationerySpot.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string PaymentMethod { get; set; }

        // إضافة مفتاح خارجي لربط العميل بالطلب
        public int OrderId { get; set; }
        public Order Order { get; set; }  // العلاقة مع الطلب
    }

}
