using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace stationerySpot.Models

{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } // Advanced Machine Learning Algorithms

        public string Description { get; set; } // Implementation of...

        public string Type { get; set; } // Master's Thesis, Graduation Project, etc.

        public string Tags { get; set; } // ممكن تخزنيهم كسلسلة مفصولة بفاصلة: "Python, ML"

        public decimal Price { get; set; } // $299

        public string ImageUrl { get; set; } // رابط الصورة

        // علاقة مع المكتبة
        [ForeignKey("Library")]
        public int LibraryId { get; set; }

        public Library Library { get; set; }
    }
}
