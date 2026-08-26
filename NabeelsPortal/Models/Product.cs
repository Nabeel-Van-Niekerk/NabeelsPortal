using System;
using System.Collections.Generic;

namespace NabeelsPortal.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string FarmerId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public DateTime ProductionDate { get; set; }

    public virtual Farmer Farmer { get; set; } = null!;
}
