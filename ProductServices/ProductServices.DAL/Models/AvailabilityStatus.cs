using System;
using System.Collections.Generic;

namespace ProductServices.DAL.Models;

public partial class AvailabilityStatus
{
    public int AvailabilityStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
