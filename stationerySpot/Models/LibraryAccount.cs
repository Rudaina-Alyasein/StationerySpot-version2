using System;
using System.Collections.Generic;

namespace stationerySpot.Models;

public partial class LibraryAccount
{
    public int Id { get; set; }

    public int LibraryId { get; set; }


    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Library Library { get; set; } = null!;
}
