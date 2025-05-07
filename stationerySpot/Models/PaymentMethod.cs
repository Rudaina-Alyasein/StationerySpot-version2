namespace stationerySpot.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string MethodName { get; set; } = null!;
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }

        // ربط مع الـ User
        public int UserId { get; set; }  // ربط الدفع بالمستخدم
        public User User { get; set; }    // كائن المستخدم المرتبط
    }
}
