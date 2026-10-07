using System;
using System.Collections.Generic;

namespace ProductServices.DAL.Models;

public partial class Category
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; } = 1;

    public bool IsDeleted { get; set; }

    public virtual CategoryStatus Status { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
