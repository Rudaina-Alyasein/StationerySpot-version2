using stationerySpot.ViewModels;

namespace stationerySpot.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }

        // عرض الطلبات
        public List<OrderViewModel> Orders { get; set; } = new();

        // طريقة الدفع المرتبطة بالمستخدم
        public PaymentMethod? PaymentMethod { get; set; }
        public Address? Address { get; set; }

        // For account editing
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
