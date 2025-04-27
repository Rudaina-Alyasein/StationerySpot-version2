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

        // ViewModels مخصصة للعرض
        public List<OrderViewModel> Orders { get; set; } = new();
        //public List<PrintRequest> PrintRequests { get; set; } = new();
        //public List<Review> Reviews { get; set; } = new();
    }

}
