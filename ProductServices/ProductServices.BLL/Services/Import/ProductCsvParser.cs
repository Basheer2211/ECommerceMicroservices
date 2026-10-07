using CsvHelper;
using ProductServices.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ProductServices.BLL.Services.Import
{
    public class ProductCsvParser : IProductCsvParser
    {
        public async Task<List<ProductImportDto>> ParseAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(
                reader,
                CultureInfo.InvariantCulture);

            var records = csv.GetRecords<ProductCsvRecord>();

            var items = new List<ProductImportDto>();

            foreach (var record in records)
            {
                if (!long.TryParse(
                        record.ProductCode,
                        out var productCode))
                {
                    throw new Exception(
                        $"Invalid ProductCode value: '{record.ProductCode}'");
                }

                if (!decimal.TryParse(
                        record.Price,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var price))
                {
                    throw new Exception(
                        $"Invalid Price value: '{record.Price}' " +
                        $"for product '{record.Name}'");
                }

                if (!int.TryParse(
                        record.Quantity,
                        out var quantity))
                {
                    throw new Exception(
                        $"Invalid Quantity value: '{record.Quantity}' " +
                        $"for product '{record.Name}'");
                }

                var categoryIds = new List<long>();

                if (!string.IsNullOrWhiteSpace(record.CategoryIds))
                {
                    foreach (var id in record.CategoryIds.Split(
                                 '|',
                                 StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!long.TryParse(
                                id.Trim(),
                                out var categoryId))
                        {
                            throw new Exception(
                                $"Invalid CategoryId '{id}' " +
                                $"for product '{record.Name}'");
                        }

                        categoryIds.Add(categoryId);
                    }
                }

                items.Add(new ProductImportDto
                {
                    ProductCode = productCode,
                    Name = record.Name,
                    Description = record.Description,
                    Price = price,
                    Quantity = quantity,
                    CategoryIds = categoryIds
                });
            }

            return items;
        }
    }
    }
