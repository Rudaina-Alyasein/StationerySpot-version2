using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace stationerySpot.Models
{
    public class OfferComment
    {
        public int Id { get; set; }

        // التعليق نفسه
        [Required]
        public string CommentText { get; set; } = null!;

        // تاريخ إضافة التعليق
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }

        // العلاقة مع الـ Offer
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; } = null!;

        // العلاقة مع الـ User (المستخدم الذي أضاف التعليق)
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
