using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace stationerySpot.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]  // تحديد طول النص
        public string Title { get; set; } = null!;

        [StringLength(500)]  // تحديد الطول الأقصى للوصف
        public string? Description { get; set; }

        [StringLength(255)]  // تحديد الطول الأقصى لمسار الصورة
        public string? ImagePath { get; set; }

        [StringLength(255)]  // تحديد الطول الأقصى للرابط
        public string? Link { get; set; }

        // 🔗 الربط مع المكتبة
        [Required]  // تأكد من أن LibraryId مطلوب
        public int LibraryId { get; set; }

        [ForeignKey("LibraryId")]
        public virtual Library Library { get; set; } = null!;

        // تاريخ انتهاء العرض
        [DataType(DataType.Date)] // تحديد نوع البيانات لتاريخ
        public DateTime? ExpiryDate { get; set; }  // يمكن أن يكون تاريخ غير مُحدد
        public virtual ICollection<OfferComment> OfferComments { get; set; } = new List<OfferComment>();

    }
}
