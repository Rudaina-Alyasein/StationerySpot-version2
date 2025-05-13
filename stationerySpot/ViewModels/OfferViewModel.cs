namespace stationerySpot.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string LibraryName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Link { get; set; }
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

    }
}
