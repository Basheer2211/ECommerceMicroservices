using ProductServices.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductServices.BLL.Services.Import
{
    public class ProductImportValidator : IProductImportValidator
    {
        public string? Validate(List<ProductImportDto> items)
        {
            if (items == null || items.Count == 0)
                return "No products to import.";

            foreach (var item in items)
            {
                if (item.ProductCode <= 0)
                {
                    return
                        $"ProductCode for product '{item.Name}' must be greater than 0.";
                }

                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    return "Product name is required.";
                }

                if (item.Price < 0)
                {
                    return
                        $"Price for product '{item.Name}' cannot be negative.";
                }

                if (item.Quantity < 0)
                {
                    return
                        $"Quantity for product '{item.Name}' cannot be negative.";
                }

                if (item.CategoryIds.Any(id => id <= 0))
                {
                    return
                        $"Invalid CategoryId for product '{item.Name}'.";
                }
            }

            return null;
        }
    }
}
