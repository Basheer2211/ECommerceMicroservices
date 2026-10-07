using System;
using System.Collections.Generic;

namespace ProductServices.DAL.Models;

public partial class Inventory
{
    public long ProductId { get; set; }

    public int Stock { get; set; }

    public virtual Product Product { get; set; } = null!;
}
