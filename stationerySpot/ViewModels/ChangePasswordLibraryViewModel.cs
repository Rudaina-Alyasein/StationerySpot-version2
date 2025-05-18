using stationerySpot.Models;
using System.ComponentModel.DataAnnotations;

namespace stationerySpot.ViewModels
{
    public class ChangePasswordLibraryViewModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm the new password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }



}
