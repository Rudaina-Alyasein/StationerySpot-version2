using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace stationerySpot.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        public string PostalCode { get; set; }
        public string Notes { get; set; }

        // علاقة مع المستخدم
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
