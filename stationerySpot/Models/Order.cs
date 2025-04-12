using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int LibraryId { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual Library Library { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
