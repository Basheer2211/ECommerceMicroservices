using System;
using System.Collections.Generic;
using System.Text;

namespace ProductServices.BLL.Services.Import
{
    public class ProductCsvRecord
    {
        public string ProductCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Price { get; set; } = string.Empty;

        public string Quantity { get; set; } = "0";

        public string? CategoryIds { get; set; }
    }
}
