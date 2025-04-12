using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class ContactU
{
    public int Id { get; set; }

    public int? SenderId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int ReceiverId { get; set; } // ✅ بدل TargetType و TargetId

    public DateTime? CreatedAt { get; set; }

    public virtual User? Sender { get; set; }

    public virtual Library? Receiver { get; set; } // ✅ بدل Target
}
