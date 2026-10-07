using System;
using System.Collections.Generic;

namespace ProductServices.DAL.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public long ProductCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int AvailabilityStatusId { get; set; } = 1;

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual AvailabilityStatus AvailabilityStatus { get; set; } = null!;

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
