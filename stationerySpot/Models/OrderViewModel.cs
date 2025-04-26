namespace stationerySpot.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
