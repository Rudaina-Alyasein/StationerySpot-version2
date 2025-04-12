using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class PrintRequest
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int LibraryId { get; set; }

    public string DocumentPath { get; set; } = null!;

    public int Copies { get; set; }

    public string Color { get; set; } = null!;

    public string PrintSide { get; set; } = null!;

    public string? Message { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Library Library { get; set; } = null!;
}
