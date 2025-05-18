using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
    public int LibraryId { get; set; } // المفتاح الأجنبي

    public Library Library { get; set; } = null!; // العلاقة

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
