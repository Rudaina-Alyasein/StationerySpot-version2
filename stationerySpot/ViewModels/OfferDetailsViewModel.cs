using stationerySpot.Models;

namespace stationerySpot.ViewModels
{
    public class OfferDetailsViewModel
    {
        public Offer Offer { get; set; } = null!;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
