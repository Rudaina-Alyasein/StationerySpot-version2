namespace stationerySpot.Models
{
    public class LibraryViewModel
    {
        public Library Library { get; set; }
        public List<Product> Products { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Service> Services { get; set; }
        public List<PrintRequest> PrintRequests { get; set; }  // إضافة الطلبات للطباعة
        public LibraryAccount LibraryAccounts { get; set; }  // مفرد مش List


    }
}
