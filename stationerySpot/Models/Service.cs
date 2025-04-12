using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class Service
{
    public int Id { get; set; }

    public int LibraryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Library Library { get; set; } = null!;
}
