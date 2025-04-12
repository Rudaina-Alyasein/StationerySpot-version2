using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace stationerySpot.Models
{
    public partial class LibraryRegistrationRequest
    {
        public int Id { get; set; }

        public string LibraryName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Services { get; set; } = null!;

        public string Phone { get; set; } = null!;

        [NotMapped]
        public IFormFile LogoPathFile { get; set; } = null!;

        public string? LogoPath { get; set; }

        public string? WorkingHours { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? Description { get; set; }

        public string? City { get; set; } // إضافة حقل المدينة

        public string Status { get; set; } = "Pending";

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
