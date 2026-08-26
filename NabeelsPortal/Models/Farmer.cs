using System;
using System.Collections.Generic;

namespace NabeelsPortal.Models;

public partial class Farmer
{
    public string FarmerId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ContactInfo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
