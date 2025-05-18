using stationerySpot.Models;

namespace stationerySpot.ViewModels
{
    public class LibraryDashboardViewModel
    {
        public Library Library { get; set; } = new Library();

        public ChangePasswordLibraryViewModel ChangePassword { get; set; } = new ChangePasswordLibraryViewModel();
    }
}
