using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace stationerySpot.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]  
        public string Title { get; set; } = null!;

        [StringLength(500)]  
        public string? Description { get; set; }

        [StringLength(255)]  
        public string? ImagePath { get; set; }

        [StringLength(255)]  
        public string? Link { get; set; }

        [Required]  
        public int LibraryId { get; set; }

        [ForeignKey("LibraryId")]
        public virtual Library Library { get; set; } = null!;

        [DataType(DataType.Date)] 
        public DateTime? ExpiryDate { get; set; }  
        public virtual ICollection<OfferComment> OfferComments { get; set; } = new List<OfferComment>();

    }
}
