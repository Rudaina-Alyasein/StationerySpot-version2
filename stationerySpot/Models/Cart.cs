using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsCheckedOut { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
