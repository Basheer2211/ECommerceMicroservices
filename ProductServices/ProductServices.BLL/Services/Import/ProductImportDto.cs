using System;
using System.Collections.Generic;
using System.Text;

namespace ProductServices.BLL.Services.Import
{
    public class ProductImportDto
    {
        public long ProductCode { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        public List<long> CategoryIds { get; set; } = new();
    }
}
