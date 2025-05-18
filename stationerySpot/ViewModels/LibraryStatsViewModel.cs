namespace stationerySpot.ViewModels
{
    public class LibraryStatsViewModel
    {
        public int TotalRequests { get; set; }
        public int NewRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int DoneRequests { get; set; }
        public int CancelledRequests { get; set; }

        // لإظهار البيانات على مدار الأيام
        public Dictionary<string, int> RequestsPerDay { get; set; } = new();

        // إحصائيات المنتجات حسب الفئة
        public List<string> CategoryNames { get; set; } = new();
        public List<int> ProductCounts { get; set; } = new();
        // 👇 New Order stats:
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalSalesAmount { get; set; }
    }

}
