using System;
using System.Collections.Generic;

namespace ProductServices.DAL.Models;

public partial class CategoryStatus
{
    public int StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
