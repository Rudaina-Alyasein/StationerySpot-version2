using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int LibraryId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }
    public string? Reply { get; set; }  // هنا خاصية الرد
    public string Status { get; set; } = "Pending";
    public virtual Library Library { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
